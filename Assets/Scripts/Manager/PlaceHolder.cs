using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHolder
{
	private static PlaceHolder _instance = null;
    public static PlaceHolder instance { get { return getInstance(); } }

	private Dictionary<string, object> placeHolder;

	private PlaceHolder() { }

    private static PlaceHolder getInstance()
    {
		if (_instance == null)
		{
			_instance = new PlaceHolder();
			_instance.placeHolder = new Dictionary<string, object>();
		}
		return _instance;
	}

    public void insert(string key, object value)
	{
		placeHolder.Add(key, value);
	}

	public void remove(string key)
	{
		placeHolder.Remove(key);
	}

	public object find(string key)
	{
		if (placeHolder.ContainsKey(key))
		{
			return placeHolder[key];
		}
		return null;
	}

	public T find<T>(string key)
	{
		if (placeHolder.ContainsKey(key))
		{
			return (T)placeHolder[key];
		}
		return default(T);
	}
}
