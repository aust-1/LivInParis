namespace LivinParis.Models.Maps;

//HACK: refactor

/// <summary>
/// Represents a station on Paris intra-muros metro network.
/// </summary>
public struct Station
{
    #region Constants

    private const int R = 6371;
    private const double DEGRE_TO_RAD = Math.PI / 180;

    #endregion Constants
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
    /// The longitude of the station in radians.
    /// </summary>
    private readonly double _longitude;

    /// <summary>
    /// The latitude of the station in radians.
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

    /// <summary>
    /// Initializes a new instance of the <see cref="Station"/> struct.
    /// </summary>
    /// <param name="line">The line the station is on.</param>
    /// <param name="name">The name of the station.</param>
    /// <param name="longitude">The longitude of the station in degrees.</param>
    /// <param name="latitude">The latitude of the station in degrees.</param>
    /// <param name="commune">The commune the station is in.</param>
    /// <param name="insee">The INSEE code of the commune the station is in.</param>
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
        _longitude = longitude * DEGRE_TO_RAD;
        _latitude = latitude * DEGRE_TO_RAD;
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

    public double GetTimeTo(Station station)
    {
        return GetDistanceTo(station) / GetSpeedByLine(_line);
    }

    private double GetDistanceTo(Station station)
    {
        return 2
            * R
            * Math.Asin(
                Math.Sqrt(
                    Math.Pow(Math.Sin((_latitude - station.Latitude) / 2), 2)
                        + Math.Cos(_latitude)
                            * Math.Cos(station.Latitude)
                            * Math.Pow(Math.Sin((_longitude - station.Longitude) / 2), 2)
                )
            );
    }

    public override string ToString()
    {
        return $"{_name} ({_line})";
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

    private static double GetSpeedByLine(string line)
    {
        switch (line)
        {
            case "1":
                return 30;
            case "2":
                return 21.6;
            case "3":
                return 22.5;
            case "3bis":
                return 19;
            case "4":
                return 21.6;
            case "5":
                return 25.9;
            case "6":
                return 26.3;
            case "7":
                return 28.1;
            case "7bis":
                return 23;
            case "8":
                return 26.9;
            case "9":
                return 22.6;
            case "10":
                return 25.1;
            case "11":
                return 28.1;
            case "12":
                return 27.2;
            case "13":
                return 39;
            case "14":
                return 40;
            default:
                throw new ArgumentException("Invalid line");
        }
    }

    #endregion Methods
}
