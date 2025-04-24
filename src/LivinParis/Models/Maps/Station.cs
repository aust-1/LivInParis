namespace LivInParisRoussilleTeynier.Models.Maps;

/// <summary>
/// Represents a station in the Paris intra-muros metro network.
/// </summary>
public class Station
{
    #region Constants

    /// <summary>
    /// The average radius of the Earth in kilometers.
    /// </summary>
    private const int EARTH_RADIUS_KM = 6371;

    /// <summary>
    /// Converts degrees to radians by multiplying by this factor.
    /// </summary>
    private const double DEGREES_TO_RADIANS = Math.PI / 180.0;

    /// <summary>
    /// Converts hours to minutes by multiplying by this factor.
    /// </summary>
    private const double HOURS_TO_MINUTES = 60.0;

    #endregion Constants

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
    private readonly double _longitudeRadians;

    /// <summary>
    /// The latitude of the station in radians.
    /// </summary>
    private readonly double _latitudeRadians;

    /// <summary>
    /// The commune where the station is located.
    /// </summary>
    private readonly string _commune;

    /// <summary>
    /// The INSEE code of the commune the station is in.
    /// </summary>
    private readonly int _insee;

    /// <summary>
    /// The color of the line the station is on.
    /// </summary>
    private readonly string _lineColor;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Station"/> struct.
    /// Longitude and latitude are converted from degrees to radians internally.
    /// </summary>
    /// <param name="line">The metro line on which this station is located (e.g., "1", "4").</param>
    /// <param name="name">The name of the station (e.g., "Châtelet").</param>
    /// <param name="longitude">The longitude in degrees.</param>
    /// <param name="latitude">The latitude in degrees.</param>
    /// <param name="commune">The commune where this station is located.</param>
    /// <param name="insee">The INSEE code of the commune.</param>
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
        _longitudeRadians = longitude * DEGREES_TO_RADIANS;
        _latitudeRadians = latitude * DEGREES_TO_RADIANS;
        _commune = commune;
        _insee = insee;
        _lineColor = GetLineColor(line);
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Gets the latitude of the station in radians.
    /// </summary>
    public double LatitudeRadians
    {
        get { return _latitudeRadians; }
    }

    /// <summary>
    /// Gets the longitude of the station in radians.
    /// </summary>
    public double LongitudeRadians
    {
        get { return _longitudeRadians; }
    }

    /// <summary>
    /// Gets the color associated with the station's metro line.
    /// </summary>
    public string LineColor
    {
        get { return _lineColor; }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Calculates the travel time (in minutes) from this station to another station.
    /// </summary>
    /// <param name="other">The target station.</param>
    /// <returns>The travel time in minutes.</returns>
    public double GetTimeTo(Station other)
    {
        double distanceKm = GetDistanceTo(other);
        double speedKmh = GetLineSpeed(_line);
        return distanceKm / speedKmh * HOURS_TO_MINUTES;
    }

    /// <summary>
    /// Calculates the great-circle distance (in kilometers) to another station
    /// using the Haversine formula.
    /// </summary>
    /// <param name="other">The target station.</param>
    /// <returns>The distance in kilometers.</returns>
    private double GetDistanceTo(Station other)
    {
        return 2.0
            * EARTH_RADIUS_KM
            * Math.Asin(
                Math.Sqrt(
                    Math.Pow(Math.Sin((_latitudeRadians - other._latitudeRadians) / 2.0), 2.0)
                        + Math.Cos(_latitudeRadians)
                            * Math.Cos(other._latitudeRadians)
                            * Math.Pow(
                                Math.Sin((_longitudeRadians - other._longitudeRadians) / 2.0),
                                2.0
                            )
                )
            );
    }

    /// <summary>
    /// Calculates the great-circle distance (in kilometers) to another station
    /// using the Haversine formula.
    /// </summary>
    /// <param name="longitude">The longitude of the target point in degrees.</param>
    /// <param name="latitude">The latitude of the target point in degrees.</param>
    /// <returns>The distance in kilometers.</returns>
    public double GetDistanceTo(double longitude, double latitude)
    {
        double otherLongitudeRadians = longitude * DEGREES_TO_RADIANS;
        double otherLatitudeRadians = latitude * DEGREES_TO_RADIANS;
        return 2.0
            * EARTH_RADIUS_KM
            * Math.Asin(
                Math.Sqrt(
                    Math.Pow(Math.Sin((_latitudeRadians - otherLatitudeRadians) / 2.0), 2.0)
                        + Math.Cos(_latitudeRadians)
                            * Math.Cos(otherLatitudeRadians)
                            * Math.Pow(
                                Math.Sin((_longitudeRadians - otherLongitudeRadians) / 2.0),
                                2.0
                            )
                )
            );
    }

    /// <summary>
    /// Returns a string representing this station, displaying the name and line.
    /// </summary>
    /// <returns>A formatted string with station name and line.</returns>
    public override string ToString()
    {
        return $"{_name} ({_line})";
    }

    /// <summary>
    /// Gets the hex color code associated with a given metro line.
    /// </summary>
    /// <param name="line">The metro line (e.g., "1", "3bis").</param>
    /// <returns>A string representing a hex color code (e.g., "#FFCE00").</returns>
    private static string GetLineColor(string line)
    {
        return line switch
        {
            "1" => "#FFCE00",
            "2" => "#0064B0",
            "3" => "#9F9825",
            "3bis" => "#98D4E2",
            "4" => "#C04191",
            "5" => "#F28E42",
            "6" => "#83C491",
            "7" => "#F3A4BA",
            "7bis" => "#83C491",
            "8" => "#CEADD2",
            "9" => "#D5C900",
            "10" => "#E3B32A",
            "11" => "#8D5E2A",
            "12" => "#00814F",
            "13" => "#98D4E2",
            "14" => "#662483",
            _ => "#000000",
        };
    }

    /// <summary>
    /// Gets the commercial speed (km/h) of the specified line.
    /// </summary>
    /// <param name="line">The metro line identifier (e.g., "1", "14").</param>
    /// <returns>The speed in kilometers per hour (km/h).</returns>
    /// <exception cref="ArgumentException">Thrown if the provided line is invalid or unsupported.</exception>
    private static double GetLineSpeed(string line)
    {
        return line switch
        {
            "1" => 30.0,
            "2" => 21.6,
            "3" => 22.5,
            "3bis" => 19.0,
            "4" => 21.6,
            "5" => 25.9,
            "6" => 26.3,
            "7" => 28.1,
            "7bis" => 23.0,
            "8" => 26.9,
            "9" => 22.6,
            "10" => 25.1,
            "11" => 28.1,
            "12" => 27.2,
            "13" => 39.0,
            "14" => 40.0,
            _ => throw new ArgumentException("Invalid line"),
        };
    }

    #endregion Methods
}
