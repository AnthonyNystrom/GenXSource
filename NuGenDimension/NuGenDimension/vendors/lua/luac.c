/*
** $Id: luac.c,v 1.28 2000/11/06 20:06:27 lhf Exp $
** lua compiler (saves bytecodes to files; also list binary files)
** See Copyright Notice in lua.h
*/

#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "lparser.h"
#include "lstate.h"
#include "lzio.h"
#include "luac.h"

#define	OUTPUT	"luac.out"		/* default output file */

static int usage(const char* message, const char* arg);
static int doargs(int argc, const char* argv[]);
static Proto* load(const char* filename);
static FILE* efopen(const char* name, const char* mode);
static void strip(Proto* tf);
static Proto* combine(Proto** P, int n);

lua_State* lua_state=NULL;		/* lazy! */

static int listing = 0;			/* list bytecodes? */
static int dumping = 1;			/* dump bytecodes? */
static int stripping = 0;			/* strip debug information? */
static const char* output=OUTPUT;	/* output file name */

#define	IS(s)	(strcmp(argv[i],s)==0)

int lc_compile(int argc, const char* argv[]) {
  Proto** P,*tf;
  FILE* f;
  int i;
  listing = 0;			/* list bytecodes? */
  dumping = 1;			/* dump bytecodes? */
  stripping = 0;		/* strip debug information? */
  i = doargs(argc,argv);
  if(i < 0)
    return 0;
  argc -= i;
  argv += i;
  if(argc <= 0)
    return usage("no input file given",NULL);
  L = lua_open(0);
  P = luaM_newvector(L,argc,Proto*);
  for(i = 0; i < argc; i++)
    P[i] = load(argv[i]);
  tf = combine(P,argc);
  if(tf == NULL)
    return 1;
  if(dumping) luaU_optchunk(tf);
  if(listing) luaU_printchunk(tf);
  if(dumping) {
    if(stripping)
      strip(tf);
    f = efopen(output,"wb");
    if(f == NULL) return 1;
    luaU_dumpchunk(tf,f);
    fclose(f);
  }
  return 0;
}

static int usage(const char* message, const char* arg) {
  if(message != NULL) {
    sc_printf("luac: ");
    if(arg != NULL)
      sc_printf(message,arg);
    else
      sc_printf(message);
    sc_printf("\n");
  }
  sc_printf("usage: luac [options] [filenames].  Available options are:\n");
  sc_printf("  -l       list\n");
  sc_printf("  -o file  output file (default is \"" OUTPUT "\")\n");
  sc_printf("  -p       parse only\n");
  sc_printf("  -s       strip debug information\n");
  sc_printf("  -v       show version information\n");
  return -1;
}

static int doargs(int argc, const char* argv[]) {
  int i;
  for(i = 1; i < argc; i++) {
    if(*argv[i] != '-')   /* end of options */
      break;
    else if(IS("-l"))     /* list */
      listing = 1;
    else if(IS("-o"))     /* output file */
    {
      output = argv[++i];
      if(output == NULL)
        return usage(NULL,NULL);
    }
    else if(IS("-p"))			/* parse only */
      dumping = 0;
    else if(IS("-s"))			/* strip debug information */
      stripping = 1;
    else if(IS("-v"))			/* show version */
    {
      sc_printf("%s  %s\n",LUA_VERSION,LUA_COPYRIGHT);
      if(argc == 2)
        return -1;
    }
    else					/* unknown option */
      return usage("unrecognized option `%s'",argv[i]);
  }
  if(i == argc && listing) {
    dumping = 0;
    argv[--i] = OUTPUT;
  }
  return i;
}

static Proto* load(const char* filename) {
  Proto* tf;
  ZIO z;
  char source[512];
  FILE* f;
  int c,undump;
  if(filename == NULL)
    return NULL;
  else
    f = efopen(filename,"r");
  if(f == NULL)
    return NULL;
  c = ungetc(fgetc(f),f);
  if(ferror(f)) {
    sc_printf("luac: cannot read from %s",filename);
    fclose(f);
    return NULL;
  }
  undump = (c == ID_CHUNK);
  if(undump) {
    fclose(f);
    f = efopen(filename,"rb");
    if(f == NULL)
      return NULL;
  }
  sprintf(source,"@%.*s",Sizeof(source)-2,filename);
  luaZ_Fopen(&z,f,source);
  tf = undump ? luaU_undump(L,&z) : luaY_parser(L,&z);
  fclose(f);
  return tf;
}

static Proto* combine(Proto** P, int n) {
  if (n==1)
    return P[0];
  else {
    int i, pc = 0;
    Proto* tf = luaF_newproto(L);
    tf->source = luaS_new(L,"=(luac)");
    tf->maxstacksize = 1;
    tf->kproto = P;
    tf->nkproto = n;
    tf->ncode = 2*n+1;
    tf->code = luaM_newvector(L,tf->ncode,Instruction);
    for(i = 0; i < n; i++) {
      tf->code[pc++] = CREATE_AB(OP_CLOSURE,i,0);
      tf->code[pc++] = CREATE_AB(OP_CALL,0,0);
    }
    tf->code[pc++]=OP_END;
    return tf;
  }
}

static void strip(Proto* tf) {
  int i, n = tf->nkproto;
  tf->lineinfo = NULL;
  tf->nlineinfo = 0;
  tf->source = luaS_new(L,"=(none)");
  tf->locvars = NULL;
  tf->nlocvars = 0;
  for(i = 0; i < n; i++) strip(tf->kproto[i]);
}

static FILE* efopen(const char* name, const char* mode) {
  FILE* f = fopen(name,mode);
  if(f == NULL) {
    sc_printf("luac: cannot open %sput file ", *mode=='r' ? "in" : "out");
    return NULL;
  }
  return f;
}
