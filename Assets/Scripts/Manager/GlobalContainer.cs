using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalContainer
{
	private static GlobalContainer _instance = null;
    public static GlobalContainer instance { get { return getInstance(); } }

	private Dictionary<string, object> container;

	private GlobalContainer() { }

    private static GlobalContainer getInstance()
    {
		if (_instance == null)
		{
			_instance = new GlobalContainer();
			_instance.container = new Dictionary<string, object>();
		}
		return _instance;
	}

    public static void store(string key, object value)
	{
		if (contains(key)) instance.container[key] = value;
		else instance.container.Add(key, value);
	}

	public static void erase(string key)
	{
		if (!contains(key)) return;
        instance.container.Remove(key);
	}

	public static bool contains(string key)
	{
		if (instance.container.ContainsKey(key))
		{
			return true;
		}
		return false;
	}

	public static Dictionary<string, object> get()
	{
		return instance.container;
	}

	public static object load(string key)
	{
		return instance.container[key];
	}

	public static T load<T>(string key)
	{
		return (T)instance.container[key];
	}

	public static bool tryLoad<T>(string key, out T value) 
	{
		if (!instance.container.ContainsKey(key))
		{
			value = default;
			return false;
		}

		value = load<T>(key);
		return true;
	}

	public static T tryLoadOrStore<T>(string key, T defaultValue)
	{
		if (!tryLoad<T>(key, out var result))
		{
			store(key, defaultValue);
			return defaultValue;
		}
		else
		{
			return result;
		}
	}

	public static void clear()
	{
		instance.container.Clear();
	}
}
