## Using SettingsKit with JSON files.

### Reading JSON files.
Each JSON file used for SettingsKit stores data in the format of an Array of KeyValuePair<TKey, TValue>.

Each key must be declared along with the value it represents within a pair of curley brackets.

Values can be stored as as any object type, hence the TValue type, but it must consistently be the same throughout the JSON file.

Key and Value declarations are case sensitive except for the "Key" and "Value" parts. Whilst they should have the first letter capitalized, SettingsKit can overlook them being lowercase (such as "key" or "value") but will attempt to fix this on writing to the file.

In theory a Key may be any type declared using TKey, in practice we expect the most common type will be that of string as it makes for easier manipulation.

Below is an example of a Settings file with two settings stored in the file, the "highway.speed.minimum.mph" and the "highway.speed.maximum.mph" keys with their corresponding values.

```
[{
  "Key: "highway.speed.minimum.mph",
  "Value": 40
},
{
  "Key": "highway.speed.maximum.mph"
  "Value": 70
}]
```

To read from this file, a JsonSettingsProvider is instantiated with the ``Get`` method used as follows.

This example makes a few assumptions:
* it assumes a file path and a name for the file - although these can be easily substituted for your actual file location and name.

* it assumes that the Keys and corresponding Values will be stored as strings - you can substitute these types for different types if you wish to store different types of objects.

```
string path = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Testing.json";

JsonSettingsProvider<string, string> settings = new JsonSettingsProvider<string, string>();

 var settings = settingsProvider.Get(path); ```

This returns an array of KeyValuePairs. To get the specific pair you are interested in, you can use ``SetingsManager``. 
