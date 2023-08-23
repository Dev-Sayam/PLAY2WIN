using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Calander_Panel : MonoBehaviour
{
    [SerializeField] private Transform pool, dates_parent;
    [SerializeField] private GameObject dummy_date_button;
    [SerializeField] private GameObject date_button;
    [SerializeField] private TextMeshProUGUI year_text, month_text;
    [SerializeField] private TextMeshProUGUI date_text_area;
    private int dummy_date_button_count = 6;
    private int date_button_count = 31;
    private GameObject[] dummy_date_button_pool;
    private GameObject[] date_button_pool;
    private int year, month, day;
    private int dummy_date_button_create_count = 6; //according to 01/01/01
    private int daycount_permonth;
    private int[] count = new int[31];
    private void Awake()
    {
        CreatePool();
        SetCurrentDate();
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void CreatePool()
    {
        dummy_date_button_pool = new GameObject[dummy_date_button_count];
        for (int i = 0; i < dummy_date_button_count; i++)
        {
            dummy_date_button_pool[i] = Instantiate(dummy_date_button, pool);
        }
        date_button_pool = new GameObject[date_button_count];

        for (int i = 0; i < date_button_count; i++)
        {
            count[i] = i + 1;
        }
        foreach (var item in count)
        {
            date_button_pool[item - 1] = Instantiate(date_button, pool);
            date_button_pool[item - 1].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.ToString();
            date_button_pool[item - 1].GetComponent<Button>().onClick.AddListener(() => { OnDateButtonClick(item); });
        }
    }
    private void SetCurrentDate()
    {
        year = System.DateTime.Now.Year;
        month = System.DateTime.Now.Month;
        day = System.DateTime.Now.Day;
        year_text.text = year.ToString();
        month_text.text = month.ToString();
        SetDateGrid();
        SetDateToDateTextArea();
    }
    public void OnLeftYearButtonClick()
    {
        if (year > 1)
        {
            year--;
            year_text.text = year.ToString();
            SetDateGrid();
        }
    }
    public void OnRightYearButtonClick()
    {
        year++;
        year_text.text = year.ToString();
        SetDateGrid();
    }
    public void OnLeftMonthButtonClick()
    {
        if (month > 1)
        {
            month--;
            SetDateGrid();
        }
        else if (year > 1)
        {
            month = 12;
            OnLeftYearButtonClick();
            SetDateGrid();
        }
        month_text.text = month.ToString();
    }
    public void OnRightMonthButtonClick()
    {
        month++;
        if (month > 12)
        {
            month = 1;
            OnRightYearButtonClick();
        }
        month_text.text = month.ToString();
        SetDateGrid();
    }
    private void SetDateGrid()
    {
        int daycount_of_selected_month = 0;
        if (month == 2)
        {
            if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
            {
                daycount_of_selected_month = 29;
            }
            else
            {
                daycount_of_selected_month = 28;
            }
        }
        else if (month % 2 == 0)
        {
            if (month <= 6)
            {
                daycount_of_selected_month = 30;
            }
            else
            {
                daycount_of_selected_month = 31;
            }
        }
        else
        {
            if (month <= 7)
            {
                daycount_of_selected_month = 31;
            }
            else
            {
                daycount_of_selected_month = 30;
            }
        }

        int day_index_selected_date = dayofweek(1, month, year);
        SetAllToPool();
        SetAllToDatesParent(day_index_selected_date, daycount_of_selected_month);
    }
    private void SetAllToPool()
    {
        for (int i = 0; i < dummy_date_button_pool.Length; i++)
        {
            dummy_date_button_pool[i].transform.SetParent(pool);
        }
        for (int i = 0; i < date_button_pool.Length; i++)
        {
            date_button_pool[i].transform.SetParent(pool);
        }
    }
    private void SetAllToDatesParent(int day_index_selected_date, int daycount_of_selected_month)
    {
        for (int i = 0; i < day_index_selected_date; i++)
        {
            dummy_date_button_pool[i].transform.SetParent(dates_parent);
        }
        for (int i = 0; i < daycount_of_selected_month; i++)
        {
            date_button_pool[i].transform.SetParent(dates_parent);
        }
    }
    private int dayofweek(int d, int m, int y)
    {
        int[] t = { 0, 3, 2, 5, 0, 3, 5, 1, 4, 6, 2, 4 };
        y -= (m < 3) ? 1 : 0;
        return (y + y / 4 - y / 100 + y / 400 + t[m - 1] + d) % 7; //(Year Code  + Century Code  - Leap Year Code + Month Code + Date Number) mod 7
    }
    private void OnDateButtonClick(int day)
    {
        this.day = day;
        this.gameObject.SetActive(false);
        SetDateToDateTextArea();
    }
    private void SetDateToDateTextArea()
    {
        date_text_area.text = day.ToString() + "/" + month.ToString() + "/" + year.ToString();
    }
}
