namespace LivinParis.Models.Maps;

//HACK: refactor

/// <summary>
/// Represents a station on Paris intra-muros metro network.
/// </summary>
public struct Station
{
    //QUESTION: Should this be a class instead of a struct?
    #region Fields

    /// <summary>
    /// The name of the station.
    /// </summary>
    private readonly string _name;

    /// <summary>
    /// The line the station is on.
    /// </summary>
    private readonly string _line;

    /// <summary>
    /// The longitude of the station.
    /// </summary>
    private readonly double _longitude;

    /// <summary>
    /// The latitude of the station.
    /// </summary>
    private readonly double _latitude;

    /// <summary>
    /// The commune the station is in.
    /// </summary>
    private readonly string _commune;

    /// <summary>
    /// The INSEE code of the commune the station is in.
    /// </summary>
    private readonly int _insee;

    /// <summary>
    /// The color of the line the station is on.
    /// </summary>
    private readonly string _colorLine;

    #endregion Fields

    #region Constructors

    public Station(
        string line,
        string name,
        double longitude,
        double latitude,
        string commune,
        int insee
    )
    {
        _line = line;
        _name = name;
        _longitude = longitude;
        _latitude = latitude;
        _commune = commune;
        _insee = insee;
        _colorLine = GetColorByLine(line);
    }

    #endregion Constructors

    #region Properties

    public double Latitude
    {
        get { return _latitude; }
    }

    public double Longitude
    {
        get { return _longitude; }
    }

    public string ColorLine
    {
        get { return _colorLine; }
    }

    #endregion Properties

    #region Methods

    public override string ToString()
    {
        return $"{_name} ({_line})";
        //return $"\"{_name} ({_line})\" [pos=\"{(_longitude * 100000000000000).ToString("F0")},{(_latitude * 100000000000000).ToString("F0")}!\", style=filled, fillcolor=\"{_colorLine}\"]";
    }

    private static string GetColorByLine(string line)
    {
        switch (line)
        {
            case "1":
                return "#FFCE00";
            case "2":
                return "#0064B0";
            case "3":
                return "#9F9825";
            case "3bis":
                return "#98D4E2";
            case "4":
                return "#C04191";
            case "5":
                return "#F28E42";
            case "6":
                return "#83C491";
            case "7":
                return "#F3A4BA";
            case "7bis":
                return "#83C491";
            case "8":
                return "#CEADD2";
            case "9":
                return "#D5C900";
            case "10":
                return "#E3B32A";
            case "11":
                return "#8D5E2A";
            case "12":
                return "#00814F";
            case "13":
                return "#98D4E2";
            case "14":
                return "#662483";
            default:
                return "#000000";
        }
    }

    #endregion Methods
}
