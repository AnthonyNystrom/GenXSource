/*
** $Id: print.c,v 1.32 2000/11/06 20:04:36 lhf Exp $
** print bytecodes
** See Copyright Notice in lua.h
*/

#include <stdio.h>
#include <stdlib.h>

#include "luac.h"

/* macros used in print.h, included in PrintCode */
#define P_OP(x)	sc_printf("%-11s\t",x)
#define P_NONE
#define P_AB	sc_printf("%d %d",GETARG_A(i),GETARG_B(i))
#define P_F	sc_printf("%d %d\t; %p",GETARG_A(i),GETARG_B(i),tf->kproto[GETARG_A(i)])
#define P_J	sc_printf("%d\t; to %d",GETARG_S(i),GETARG_S(i)+at+1)
#define P_Q	PrintString(tf,GETARG_U(i))
#define P_K	sc_printf("%d\t; %s",GETARG_U(i),tf->kstr[GETARG_U(i)]->str)
#define P_L	PrintLocal(tf,GETARG_U(i),at-1)
#define P_N	sc_printf("%d\t; " NUMBER_FMT,GETARG_U(i),tf->knum[GETARG_U(i)])
#define P_S	sc_printf("%d",GETARG_S(i))
#define P_U	sc_printf("%u",GETARG_U(i))

static void PrintString(const Proto* tf, int n)
{
 const char* s=tf->kstr[n]->str;
 sc_printf("%d\t; ",n);
 putchar('"');
 for (; *s; s++)
 {
  switch (*s)
  {
   case '"': sc_printf("\\\""); break;
   case '\a': sc_printf("\\a"); break;
   case '\b': sc_printf("\\b"); break;
   case '\f': sc_printf("\\f"); break;
   case '\n': sc_printf("\\n"); break;
   case '\r': sc_printf("\\r"); break;
   case '\t': sc_printf("\\t"); break;
   case '\v': sc_printf("\\v"); break;
   default: {
    sc_printf("%c",*s);
    break;
   }
  }
 }
 putchar('"');
}

static void PrintLocal(const Proto* tf, int n, int pc)
{
 const char* s=luaF_getlocalname(tf,n+1,pc); 
 sc_printf("%u",n);
 if (s!=NULL) sc_printf("\t; %s",s);
}

static void PrintCode(const Proto* tf)
{
 const Instruction* code=tf->code;
 const Instruction* p=code;
 for (;;)
 {
  int at=p-code+1;
  Instruction i=*p;
  int line=luaG_getline(tf->lineinfo,at-1,1,NULL);
  sc_printf("%6d\t",at);
  if (line>=0) sc_printf("[%d]\t",line); else sc_printf("[-]\t");
  switch (GET_OPCODE(i)) {
#include "print.h"
  }
  sc_printf("\n");
  if (i==OP_END) break;
  p++;
 }
}

#define IsMain(tf)	(tf->lineDefined==0)

#define SS(x)	(x==1)?"":"s"
#define S(x)	x,SS(x)

static void PrintHeader(const Proto* tf)
{
 sc_printf("\n%s " SOURCE_FMT " (%d instruction%s/%d bytes at %p)\n",
 	IsMain(tf)?"main":"function",SOURCE,
	S(tf->ncode),tf->ncode*Sizeof(Instruction),tf);
 sc_printf("%d%s param%s, %d stack%s, ",
	tf->numparams,tf->is_vararg?"+":"",SS(tf->numparams),S(tf->maxstacksize));
 sc_printf("%d local%s, %d string%s, %d number%s, %d function%s, %d line%s\n",
	S(tf->nlocvars),S(tf->nkstr),S(tf->nknum),S(tf->nkproto),S(tf->nlineinfo));
}

#define PrintFunction luaU_printchunk

void PrintFunction(const Proto* tf)
{
 int i,n=tf->nkproto;
 PrintHeader(tf);
 PrintCode(tf);
 for (i=0; i<n; i++) PrintFunction(tf->kproto[i]);
}
