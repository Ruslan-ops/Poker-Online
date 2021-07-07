using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerEngine
{
    class Node<T> where T : class
    {
        public Node<T> Next { get; set; }
        public T Item { get; set; }

        public Node(T item, Node<T> next)
        {
            Next = next;
            Item = item;
        }
        public Node(Node<T> next) : this(null, next) { }
        public Node(T item) : this(item, null) { }

        public Node() : this(null, null) { }
    }
    public class Ring<T> : IEnumerable<T> where T : class
    {
        public bool IsEmpty => Size == 0;
        public int Size { get; private set; }
        private Node<T> _head;


        public IEnumerator<T> GetEnumerator()
        {
            if (IsEmpty)
            {
                yield break;
            }
            Node<T> current = _head;
            yield return current.Item;
            while (current.Next != _head)
            {
                current = current.Next;
                yield return current.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Insert(int index, T item)
        {
            if (_head == null)
            {
                _head = new Node<T>(item);
                _head.Next = _head;
            }
            else
            {
                if (index <= 0)
                {
                    Prepend(item);
                    return;
                }
                else if (index >= Size)
                {
                    Append(item);
                    return;
                }
                else
                {
                    Node<T> current = _head;
                    for (int i = 0; i < index - 1; i++)
                    {
                        current = current.Next;
                    }
                    Node<T> tmp = current.Next;
                    current.Next = new Node<T>(item, tmp);
                }
            }
            Size++;
        }

        private void Append(T item)
        {
            Node<T> last = _head;
            for (int i = 0; i < Size - 1; i++)
            {
                last = last.Next;
            }
            last.Next = new Node<T>(item, _head);
            Size++;
        }

        private void Prepend(T item)
        {
            T oldHeadItem = _head.Item;
            Node<T> oldHeadNext = _head.Next;
            _head.Item = item;
            _head.Next = new Node<T>(oldHeadItem, oldHeadNext ?? _head);
            Size++;
        }


        private Node<T> FindNodeWith(T item)
        {
            CheckContains(item);
            Node<T> current = _head;
            while (current.Item != item)
            {
                current = current.Next;
            }
            return current;
        }

        private Node<T> FindNode(int index)
        {
            CheckRange(index);
            Node<T> current = _head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            return current;
        }

        public T GetNextAfter(T current)
        {
            Node<T> node = FindNodeWith(current);
            return node.Next.Item;
        }

        public Queue<T> ToQueue(T firstItem, Func<T, bool> predicate)
        {
            Node<T> current = FindNodeWith(firstItem);
            Queue<T> queue = new Queue<T>();
            for (int i = 0; i < Size; i++)
            {
                if (predicate(current.Item))
                {
                    queue.Enqueue(current.Item);
                }
                current = current.Next;
            }
            return queue;
        }

        public void Spin()
        {
            _head = _head.Next;
        }

        private void CheckEmpty()
        {
            if (IsEmpty)
            {
                throw new Exception("The ring is empty");
            }
        }

        private void CheckContains(T item)
        {
            if (!this.Contains(item))
            {
                throw new Exception("No such item");
            }
        }

        public void Remove(T item)
        {
            CheckEmpty();
            CheckContains(item);
            if (Size == 1)
            {
                _head = null;
            }
            else
            {
                Node<T> nodeBeforeDeleted = _head;
                while (nodeBeforeDeleted.Next.Item != item)
                {
                    nodeBeforeDeleted = nodeBeforeDeleted.Next;
                }
                if (nodeBeforeDeleted.Next == _head)
                {
                    _head = _head.Next;
                    nodeBeforeDeleted.Next = _head;
                }
                else
                {
                    Node<T> nodeToDelete = nodeBeforeDeleted.Next;
                    nodeBeforeDeleted.Next = nodeToDelete.Next;
                }
            }
            Size--;
        }


        public void RemoveAt(int index)
        {
            Remove(this[index]);
        }

        private void CheckRange(int index)
        {
            if (index >= Size || index < 0)
            {
                throw new Exception("index out of range");
            }

        }

       

        public T this[int index]
        {
            get
            {
                Node<T> requared = FindNode(index);
                return requared.Item;
            }
            set
            {
                Node<T> requared = FindNode(index);

                requared.Item = value;
            }
        }


    }
}
