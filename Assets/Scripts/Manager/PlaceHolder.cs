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

    public void store(string key, object value)
	{
		if (contains(key)) placeHolder[key] = value;
		else placeHolder.Add(key, value);
	}

	public void pop(string key)
	{
		if (!contains(key)) return;
		placeHolder.Remove(key);
	}

	public bool contains(string key)
	{
		if (placeHolder.ContainsKey(key))
		{
			return true;
		}
		return false;
	}

	public object load(string key)
	{
		return placeHolder[key];
	}

	public T load<T>(string key)
	{
		return (T)placeHolder[key];
	}
}
