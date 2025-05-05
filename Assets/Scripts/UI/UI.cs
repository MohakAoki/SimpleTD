using System.Collections.Generic;
using UnityEngine;

public abstract class Form : MonoBehaviour
{
    public abstract void Init();
    public abstract void Open();
    public abstract void Close();
}

public class UI : MonoBehaviour
{
    public static UI Instance;

    List<Form> forms;

    private void Awake()
    {
        Debug.Assert(Instance == null);
        Instance = this;

        Init();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Init()
    {
        forms = new List<Form>();

        for (int i = 0; i < transform.childCount; i++)
        {
            forms.Add(transform.GetChild(i).GetComponent<Form>());
            forms[i].Init();
        }
    }

    public T GetForm<T>() where T : Form
    {
        for (int i = 0; i < forms.Count; i++)
        {
            if (forms[i] is T)
                return forms[i] as T;
        }

        return null;
    }

    public void OpenForm<T>() where T : Form
    {
        GetForm<T>().Open();
    }

    public void OpenForm(Form form)
    {
        form.Open();
    }

    public void CloseForm<T>() where T : Form
    {
        GetForm<T>().Close();
    }

    public void CloseForm(Form form)
    {
        form.Close();
    }
}
