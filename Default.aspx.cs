using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
    string connectionString = "Data Source=Ajendra;Initial Catalog=EzeeMoving;Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        //calcDistance();
    }

    protected void btnCal_Click(object sender, EventArgs e)
    {
        getdistance(txtSource.Text.ToString(),txtDestination.Text.ToString());
    }

    public void getdistance(string origin, string destination)
    {
        string url = @"http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&sensor=false";

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        WebResponse response = request.GetResponse();
        Stream datastream = response.GetResponseStream();
        StreamReader sreader = new StreamReader(datastream);
        string responseReader = sreader.ReadToEnd();
        response.Close();

        DataSet ds = new DataSet();
        ds.ReadXml( new XmlTextReader(new StringReader(responseReader)));
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables["element"].Rows[0]["status"].ToString() == "OK")
            {
                lblDuration.Text = ds.Tables["duration"].Rows[0]["text"].ToString();
                lblDist.Text = ds.Tables["distance"].Rows[0]["text"].ToString();
            }
        }
    }

    public void calcDistance()
    {
        List<string> lst1 = new List<string>();
        List<string> lst2 = new List<string>();
        DataTable dt = new DataTable();
        string url = "";
        HttpWebRequest request;
        WebResponse response;
        Stream datastream;
        StreamReader sreader;
        string responseReader;
        DataSet ds;
        List<LocationMapping> lst3 = new List<LocationMapping>();

        SqlConnection cn = new SqlConnection(connectionString);
        cn.Open();
        SqlCommand cmd = new SqlCommand("select LocationDesc from locationmaster", cn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lst1.Add(dt.Rows[i]["LocationDesc"].ToString() + ", Mumbai");
                lst2.Add(dt.Rows[i]["LocationDesc"].ToString() + ", Mumbai");
            }

            for (int j = 0; j < lst1.Count ; j++)
            {
                for (int k = 0; k < lst2.Count ; k++)
                {
                    url = @"http://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + lst1[j].ToString() + "&destinations=" + lst2[k].ToString() + "&sensor=false";
                    request = (HttpWebRequest)WebRequest.Create(url);
                    response = request.GetResponse();
                    datastream = response.GetResponseStream();
                    sreader = new StreamReader(datastream);
                    responseReader = sreader.ReadToEnd();
                    response.Close();
                    ds = new DataSet();
                    ds.ReadXml(new XmlTextReader(new StringReader(responseReader)));
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables["element"].Rows[0]["status"].ToString() == "OK")
                        {
                            LocationMapping map = new LocationMapping();

                            map.Source = lst1[j].ToString();
                            map.Destination = lst2[k].ToString();
                            map.Distance = ( ds.Tables["distance"].Rows[0]["text"].ToString());
                            lst3.Add(map);
                            //lblDist.Text = ds.Tables["distance"].Rows[0]["text"].ToString();
                        }
                    }
                    Thread.Sleep(200);
                }
            }
            grdMapping.DataSource = lst3;
            grdMapping.DataBind();
        }

    }
}

partial class LocationMapping
{
    string _source = string.Empty;
    string _destination = string.Empty;
    string _distance = string.Empty;

    public string Source
    {
        get { return _source; }
        set { _source = value; }
    }
    public string Destination
    {
        get { return _destination; }
        set { _destination = value; }
    }
    public string Distance
    {
        get { return _distance; }
        set { _distance = value; }
    }
}