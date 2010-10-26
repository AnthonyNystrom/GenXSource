#pragma once

#ifndef _DEBUG
#define ARRAY_NO_BOUND
#endif

namespace RenderSurfaceArray
{

	template <class A>
	class arrayRender{
		private:
			A *top;
			int len;
		public:
			arrayRender()
			{
				top = NULL;
				len = 0;
			};

			arrayRender(int n)
			{
				len = n;

				if(len > 0)
					top = new A [n];
				else
					top = NULL;
			};
		
			arrayRender(const arrayRender<A> &copy)
			{
				top = NULL;

				(*this) = copy;
			};
			
			~arrayRender()
			{
				if(top != NULL){
					delete [] top;
					top = NULL;
				}
			};

			inline A& operator[](int i)
			{
				#ifndef ARRAY_NO_BOUND
				if((i < 0) || (i >= len))
				{
					//	throw QmolError("Index out of bounds in arrayRender!");
					TRACE("Index out of bounds in arrayRender!");
				}
				#endif // ARRAY_NO_BOUND

				return top[i];
			};

			inline A* Pointer()
			{
				return top;
			};

			arrayRender<A>& operator=(const arrayRender<A> &copy)
			{
				if(top){
					delete [] top;
				}

				len = copy.len;

				if(len > 0)
					top = new A [copy.len];
				else
					top = NULL;

				for(int i = 0;i < len;i++)
					top[i] = copy.top[i];

				return (*this);
			}

			// Initialize all elements of the arrayRender to the same
			// value. This function is needed for backwards compatibility
			// with the vector class that was used previously.
			arrayRender<A>& operator=(const A &init)
			{
				for(int i = 0;i < len;i++)
					top[i] = init;

				return (*this);
			}

			inline int Length() const 
			{
				return len;
			};

			inline void Alloc(int n)
			{
				if(top)
					delete [] top;
				
				len = n;

				if(len > 0){
					top = new A [n];

					if(top == NULL)
					{
						//	throw QmolError("Out of memory");
						TRACE("Out of memory");
					}
				}
				else{
					top = NULL;
				}
			};
	};

};

