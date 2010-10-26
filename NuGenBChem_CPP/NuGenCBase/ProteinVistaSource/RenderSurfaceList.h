//	(c) 2000-2002 J. Gans (jdg9@cornell.edu), Shalloway Lab, 
//	Cornell University. You can modify and freely distribute 
//	this code, but please keep this header intact.
//
//	Look-and-feel inspired by Xmol 
//	(http://www.networkcs.com/msc/docs/xmol/).
//
//	OpenGL selection code and quaterion rotation matrix
//	formulation from molview demo program by Mark Kilgard
//	(http://reality.sgi.com/opengl/OpenGLforX.html). PostScript
//	rendering also from Mark Kilgard.
//
//	The printing routines are based on code from
//	Craig Fahrnbach <craignan@home.com> via
//	Uwe Kotyczka <kotyczka@bnm-gmbh.de>.
//
//	Covalent bond determination code based on VMD
//	http://www.ks.uiuc.edu/Research/vmd/
//
//	Stride (http://www.embl-heidelberg.de/stride/stride.html)
//	is used for secondary structure determination and has been 
//	included in Qmol with the kind permission of 
//	Dmitrij Frishman, PhD
//	Institute for Bioinformatics
//	GSF - Forschungszentrum f? Umwelt und Gesundheit, GmbH
//	Ingolst?ter Landstra? 1,
//	D-85764 Neuherberg, Germany
//	Telephone: +49-89-3187-4201
//	Fax: +49-89-31873585
//	e-mail: d.frishman@gsf.de
//	WWW: http://mips.gsf.de/mips/staff/frishman/
//

// This class implements a doubly linked listRender.
//	-	Added read() and write() functions to handle
//		binary disk operations for the listRender class.

#pragma once

#include <stdio.h>
#include <stdlib.h>

namespace RenderSurfaceList
{
	// A forward definition for friend status
	template <class C> class ListIterator;

	template <class A>
	class listelem{
		public:
		listelem<A> *next, *prev;
		A value;
		
		listelem()
		{
			next = prev = NULL;
		};
	};

	// Template listRender implementation:
	template <class B>
	class listRender{
		private:
		listelem<B> *head;
		int length;
		
		/*
		 * Take advantage of the fact that the
		 * listRender may often be accessed sequentially.
		 */
		int last_index;
		listelem<B> *last_ptr;
		
		public:
		friend class ListIterator<B>;
		/*
		 * Perform all default initialization here.
		 */
		void Init()
		{
			length = 0; 
			last_index = 0;
			head = NULL;
		};
		
		listRender() 
		{ 
			Init();
		};
			
		~listRender() 
		{ 
			while(!IsEmpty())
				Pop();
		};
		
		/*
		 * Copy constructor
		 * This function has given problems when compiled on
		 * an AIX machine with optimization. The problem disappears
		 * when debugging is turned on (via -g) or extra print statements
		 * are added - so it may be a memory/array indexing bug.
		 */
		listRender(listRender<B>& copy)
		{
			Init();
			for(int i = 0;i < copy.length;i++)
			Push(copy[i]);
		}
		
		listRender<B>& operator=(listRender<B>& copy)
		{
			// Remove any existing elements
			while(!IsEmpty())
				Pop();
			
			for(int i = 0;i < copy.length;i++)
			Push(copy[i]);
			
			return (*this);
		}
			
		/*
		 * Make "thing" the i^{th} element of the listRender.
		 */
		void Insert(const B &thing, int i);
		
		/*
		 * Make "thing" the first elememt of the listRender.
		 */
		void Push(const B &thing);

		// Make "thing" the first elememt of the listRender.
		void PushTop(const B &thing);

		// Make "thing" the last elememt of the listRender.
		void PushTail(const B &thing);

		/*
		 * Remove the i^{th} element of the listRender.
		 */
		void Remove(int i);
		
		/*
		 * Remove the last element of the listRender.
		 * DOES NOT return the value of the popped element.
		 */
		void Pop();
		
		int Length(){return length;};
		
		int IsEmpty() {return !length;};
		
		B& operator [] (int i);
	};

	template <class B>
	void listRender<B>::Pop()
	{
		Remove(length - 1);
	}

	template <class B>
	void listRender<B>::Remove(int i)
	{
		listelem<B> *cur = last_ptr;
		int temp_index = i;

		#ifdef _DEBUG
		if((i < 0) || (i >= length))
		{
			//	throw QMOL_ERROR("Error! Attempt to remove an non-existant""element the listRender.");
			TRACE("Error! Attempt to remove an non-existant");
		}
		#endif // _DEBUG

		if(i >= last_index){
			i -= last_index;
			while(i > 0){
				cur = cur->next;
				i--;
			}
		}
		else /* (i < last_index) */{
			i = last_index - i;
			while(i > 0){
				cur = cur->prev;
				i--;
			}
		}

		// Delete the i^{th} element from ...
		// ... the middle if the listRender
		if((cur->prev != NULL) && (cur->next != NULL)){
			cur->prev->next = cur->next;
			cur->next->prev = cur->prev;
			last_ptr = cur->prev;
			last_index = temp_index - 1;
		}
		// ... the end of the listRender
		else if(cur->prev != NULL){
			cur->prev->next = NULL;
			last_ptr = cur->prev;
			last_index = temp_index - 1;
		}
		// ... the begining of the listRender.
		else if(cur->next != NULL){
			head = cur->next;
			last_ptr = cur->next;
			last_index = 0;
			cur->next->prev = NULL;
		}
		else{ /* Last element */
			last_index = 0;
			head = NULL;
		}

		length--;
		delete cur;
	}

	template <class B>
	void listRender<B>::Push(const B &thing)
	{
		// Push data onto the tail of the listRender.
		// Note that pushing onto the top of the listRender is more
		// efficient in this particular implementation of a linked
		// listRender -- PushTop should have been the default behaviour of 
		// Push (there are a few functions that depend on the order of
		// listRender pushing, so don't change without extensive testing!).
		Insert(thing, length);
	}

	template <class B>
	void listRender<B>::PushTop(const B &thing)
	{
		// Push data onto the head of the listRender
		if(length == 0){
			head = new listelem<B>;
			if(head == NULL)
			{
				//	throw QMOL_ERROR("Error! Can't allocate memory for first ""element of listRender");
				TRACE("Error! Can't allocate memory for first ");
			}

			head->value = thing;
			last_ptr = head;
			last_index = 0;
		}
		else{
			// Add to the head of the listRender
			last_ptr = new listelem<B>;
			last_ptr->next = head;
			last_ptr->prev = NULL;
			last_ptr->value = thing;
			head->prev = last_ptr;
			head = last_ptr;
			last_index = 0;
		}

		length++;
	}

	template <class B>
	void listRender<B>::PushTail(const B &thing)
	{
		// Push data onto the tail of the listRender
		Insert(thing, length);
	}

	template <class B>
	void listRender<B>::Insert(const B &thing, int i)
	{
		listelem<B> *cur = last_ptr;
    
		#ifdef _DEBUG
		if((i < 0) || (i > length))
		{
			//	throw QMOL_ERROR("Error! Attempt to add an ""element beyond the bounds of the listRender.");
			TRACE("Error! Attempt to add an element beyond the bounds of the listRender.");
		}
		#endif // _DEBUG

		// Create the first element of the listRender?
		if(length == 0){
			head = new listelem<B>;
			if(head == NULL)
			{
				//	throw QMOL_ERROR("Error! Can't allocate memory for first ""element of listRender");
				TRACE("Error! Can't allocate memory for first ");
			}

			head->value = thing;
			last_ptr = head;
			last_index = 0;
		}
		// Add to the end of the listRender
		else if(i == length){
			/* i must be greater than last_index */
			i -= last_index;
			while(i != 1){
				cur = cur->next;
				i--;
			}
				
			cur->next = new listelem<B>;
			cur->next->value = thing;
			cur->next->prev = cur;
			last_ptr = cur->next;
			last_index = length;
		}
		// Add to the head of the listRender
		else if(i == 0){
			last_ptr = new listelem<B>;
			last_ptr->next = head;
			last_ptr->prev = NULL;
			last_ptr->value = thing;
			head->prev = last_ptr;
			head = last_ptr;
			last_index = 0;
		}
		else{
    
			listelem<B> *temp;
			int temp_index = i;
				
			if(i >= last_index){
				i -= last_index;
				while(i != 0){
					cur = cur->next;
					i--;
				}
			}
			else /*(i < last_index) */{
				i = last_index - i;
				while(i != 0){
					cur = cur->prev;
					i--;
				}
			}

			temp = new listelem<B>;
			temp->value = thing;

			temp->next = cur;
			temp->prev = cur->prev;
			cur->prev = temp;
			temp->prev->next = temp;

			last_ptr = temp;
			last_index = temp_index;
		}
    
		length++;
	}

	template <class B>
	B& listRender<B>::operator [] (int i)
	{
		listelem<B> *cur = last_ptr;
		int temp_index = i;
    
		#ifdef _DEBUG
		if((i < 0) || (i >= length))
		{
			//	throw QMOL_ERROR("Error! List index out of bounds!.");
			TRACE("Error! List index out of bounds!.");
		}
		#endif // _DEBUG

		if(i >= last_index){
			i -= last_index;
			// Go to the i^{th} element of the listRender.
			while(i != 0){
				cur = cur->next;
				i--;
			}
		}
		else /* (i < last_index) */{
			i = last_index - i;
			// Go to the i^{th} element of the listRender.
			while(i != 0){
				cur = cur->prev;
				i--;
			}
		}
    
		last_ptr = cur;
		last_index = temp_index;
    
		return cur->value;
	}

	// Provide sequential listRender access
	template <class C>
	class ListIterator{
	private:
		int index;
		listelem<C> *cur;
		listelem<C> *last;
	public:
		ListIterator(listRender<C> &lpList)
		{
			Init(lpList);
		};

		void Init(listRender<C> &lpList)
		{
			cur = lpList.head;
			last = NULL;
			index = 0;
		};

		C* operator()(int &m_index)
		{
			if(cur == NULL){
				return NULL;
			}

			// Save and increment the index
			m_index = index;

			index ++;

			// Save the returned element as the last
			// element acessed.
			listelem<C> *ret_value = last = cur;

			cur = cur->next;

			return &(ret_value->value);
		}

		C* operator()()
		{
			if(cur == NULL){
				return NULL;
			}

			// Increment the index
			index ++;

			// Save the returned element as the last
			// element acessed.
			listelem<C> *ret_value = last = cur;

			cur = cur->next;

			return &(ret_value->value);
		}

		void RemoveCurrent(listRender<C> &lpList)
		{
			if(last == NULL){
				return;
			}
			
			listelem<C> *tmp = last;

			if(tmp->prev){
				if(tmp->next){
					tmp->prev->next = tmp->next;
					tmp->next->prev = tmp->prev;
				}
				else{ // tmp->next == NULL
					tmp->prev->next = NULL;
				}
			}
			else{ // tmp->prev == NULL
				if(tmp->next){
					tmp->next->prev = NULL;
				}
			}

			if(tmp == lpList.head){
				// We are removing the head pointer
				lpList.head = tmp->next;
			}

			// The user will not be allowed to delete elements with 
			// RemoveCurrent(...) until the operator () or Init() is
			// called again.
			last = NULL;

			lpList.length --;
			index --;
			
			// Make sure that we don't break the expected behavior of
			// operator [] in case a user starts mixing calls to the
			// iterator and the [] function.
			lpList.last_index = 0;
			lpList.last_ptr = lpList.head;

			delete tmp;
		}
	};
};


