namespace WorldServer.Utility;

/// <summary>
/// 풀에서 생성되고 해제 될때 이벤트를 받고 싶다면 해당 인터페이스의 구현체로 디자인 해야한다.
/// </summary>
public interface IPoolItem
{
	void OnCreate();

	void OnReturn();
}

/// <summary>
/// 기본적인 Heap Object들의 Pooling을 지원하는 Pool 클래스
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPool<T> : IDisposable
	where T : class, new()
{
	/// <summary>
	/// pool이 바닥 났을때 할당할 사이즈
	/// </summary>
	private static int AllocSize => 32;

	private readonly Queue<T> pool = new Queue<T>();

	public ObjectPool(int capacity = 32)
	{
		if (capacity <= 0)
		{
			capacity = 32;
		}

		for (int i = 0; i < capacity; i++)
		{
			pool.Enqueue(new T());
		}
	}

	public T Create()
	{
		if (pool.Count == 0)
		{
			for (int i = 0; i < AllocSize; i++)
			{
				pool.Enqueue(new T());
			}
		}

		var result = pool.Dequeue();

		if (result is IPoolItem poolItem)
		{
			poolItem.OnCreate();
		}

		return result;
	}

	public void Return(T item)
	{
		if (item is IPoolItem poolItem)
		{
			poolItem.OnReturn();
		}

		pool.Enqueue(item);
	}

	public void Dispose()
	{
		pool.Clear();
	}
}
