/* 
 * Author:  Samuel Arzt
 * Date:    2016.06.09
 */

using System;
using System.Collections.Generic;

namespace A_Star_Demo
{
    /// <summary>
    /// Represents a binary Min-Heap by encapsulating an array.
    /// </summary>
    /// <typeparam name="T">The Type of the elements to be stored in the heap</typeparam>
    class BinaryHeap<T> where T : IComparable<T>
    {

        public int Count
        {
            get { return arr.Count; }
        }

        private List<T> arr;

        public T this[int idx]
        {
            get { return arr[idx]; }
        }


        public BinaryHeap()
        {
            arr = new List<T>();
        }
        public BinaryHeap(int capacity)
        {
            arr = new List<T>(capacity);
        }

        /// <summary>
        /// Adds a new element to the heap by performing upheap operations until the element is at the right position.
        /// </summary>
        /// <param name="element">The new element to be added to the heap.</param>
        public void Add(T element)
        {
            arr.Add(element);
            Upheap(arr.Count - 1);
        }

        /// <summary>
        /// Removes the root of the heap (the smallest element in Min-Heap)
        /// and reorders the heap to a non conflicting state.
        /// </summary>
        /// <returns>The element that was removed from the heap (the previous root).</returns>
        public T Remove()
        {
            if (arr.Count <= 0)
                throw new InvalidOperationException("The heap is empty!");

            T root = arr[0];
            if (arr.Count > 1)
            {
                //Reorder heap
                Swap(0, arr.Count - 1);
                arr.RemoveAt(arr.Count - 1);
                Downheap(0);
            }
            else
                arr.RemoveAt(0);

            return root;
        }

        /// <summary>
        /// Finds the first element that returns 0 when compared with given comparer.
        /// </summary>
        /// <param name="query">The element to query with.</param>
        /// <param name="comparer">The comparer for finding sought element.</param>
        /// <returns>The index first element that returns 0 when compared with given comparer, 
        /// or -1 if no such element exists.</returns>
        public int Find(T query, IComparer<T> comparer)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                if (comparer.Compare(arr[i], query) == 0)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Replaces the element at given index with given element, reordering the heap afterwards.
        /// </summary>
        /// <param name="update">The element to replace the old element with.</param>
        /// <param name="idx">The index of the old element to be replaced.</param>
        public void Replace(T update, int idx)
        {
            if (idx < 0 || idx >= arr.Count)
                throw new ArgumentOutOfRangeException("index is out of heap range.");

            T oldElem = arr[idx];
            arr[idx] = update;
            //Check if down- or upheap is necessary
            int compare = oldElem.CompareTo(update);
            if (compare > 0) //old element was bigger than new one
                Upheap(idx);
            else if (compare < 0) //old element was smaller than new one
                Downheap(idx);
        }

        //Swaps to elements in the array
        private void Swap(int idx1, int idx2)
        {
            T tempCopy = arr[idx1];
            arr[idx1] = arr[idx2];
            arr[idx2] = tempCopy;
        }

        //returns the index of the parent node
        private static int ParentIdx(int idx)
        {
            return (idx - 1) / 2;
        }

        //returns the index of the left child 
        private static int LeftChildIdx(int idx)
        {
            return (2 * idx + 1);
        }

        //returns the index of the right child
        private static int RightChildIdx(int idx)
        {
            return (2 * idx + 2);
        }

        //Performs upheap operation on given index until it is in right position in the heap
        private void Upheap(int idx)
        {
            //swap if the parent is bigger
            int compare = arr[ParentIdx(idx)].CompareTo(arr[idx]);

            while (idx > 0 &&
                (arr[ParentIdx(idx)].CompareTo(arr[idx]) > 0))
            {
                Swap(ParentIdx(idx), idx);
                idx = ParentIdx(idx);
            }
        }

        //Performs downheap operations on given index until it is at the right position in the heap
        private void Downheap(int idx)
        {
            while (true)
            { //while not at the back
              //find smaller child
                int childIdx = LeftChildIdx(idx);
                if (childIdx >= arr.Count) break; //break if at the end
                if (RightChildIdx(idx) < arr.Count && 
                    arr[RightChildIdx(idx)].CompareTo(arr[LeftChildIdx(idx)]) < 0)
                    childIdx = RightChildIdx(idx);
                // if the child is smaller, do the swap, otherwise break
                if (arr[childIdx].CompareTo(arr[idx]) < 0)
                {
                    Swap(childIdx, idx);
                    idx = childIdx;
                }
                else break;
            }
        }


        /// <summary>
        /// Prints the contents of this heap in level order to console.
        /// </summary>
        public void Print()
        {
            foreach (T elem in arr)
                Console.Write(elem + ", ");
            Console.WriteLine();
        }
    }
}
