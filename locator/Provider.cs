using System.Text.Json;
using System.Text.Json.Serialization;

public class Geopoint
{
    public string type { get; set; }
    public List<double> coordinates { get; set; }
}

public class Provider
{
    public int provider_id { get; set; }
    public string provider_name { get; set; }
    public string address1 { get; set; }
    public string address2 { get; set; }
    public string city { get; set; }
    public string state { get; set; }
    public string zip { get; set; }
    public string latitude { get; set; }
    public string longitude { get; set; }
    public Geopoint geopoint { get; set; }
    public string last_report_date { get; set; }
    public bool is_pap { get; set; }
    public bool is_telehealth { get; set; }
    public bool pharmacist_prescribing { get; set; }
    public bool home_delivery { get; set; }
    public bool is_t2t_site { get; set; }
    public bool is_icatt_site { get; set; }
    public bool has_usg_product { get; set; }
    public bool has_commercial_product { get; set; }
    public bool has_paxlovid { get; set; }
    public bool has_commercial_paxlovid { get; set; }
    public bool has_usg_paxlovid { get; set; }
    public bool has_lagevrio { get; set; }
    public bool has_commercial_lagevrio { get; set; }
    public bool has_usg_lagevrio { get; set; }
    public bool has_veklury { get; set; }
    public string grantee_code { get; set; }

    [JsonPropertyName(":@computed_region_pqdx_y6mm")]
    public string computed_region_pqdx_y6mm { get; set; }
    public string public_phone { get; set; }
    public TelehealthWebsite telehealth_website { get; set; }
    public string provider_note { get; set; }
}

public class TelehealthWebsite
{
    public string url { get; set; }
}

