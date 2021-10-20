## Roadmap
Potential future Roadmap of SettingsKit

### 3.1
* Start migration away from ``Preference<TKey, TValue>`` - All previous functionality has been moved to ``KeyValueDataStore`` and it nows inherits ``KeyValueDataStore``.
* Add support for migrating between ``KeyValueDataStore`` changes or versions in the future.

### 3.2
* Add support for Multi Dimensional Settings/Splitting up settings into separate files.

### 3.3
* Add support for saving settings as XML files.

### 3.4
* Add support for saving settings as TXT files.

### 3.5
* Add support for Settings File(s) name obfuscation.
* Add support for Settings File(s) contents obfuscation.

### 3.6
* Add support for Settings File(s) encryption.

### 3.7
* Add support for temporary Settings Files that are only intended to exist during app runtime.

### 3.8
*

### 4.0
* Removed support for ``Preference<TKey, TValue>`` class in it's current form.
