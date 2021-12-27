Password Strength Indicator using jQuery and XML

By: Bryian Tan (bryian.tan@hotmail.com)

12-22-2016 - v02.01.00 //check keyboard sequence
12-01-2015 - v02.00.00
07-23-2012 - v01.03.00
01-17-2011 - v01.02.00
01-12-2011 - v01.01.00

Password Strength Indicator somewhat similar to AJAX PasswordStrength extender control behavior and implemented by using jQuery and XML.
Available Features:
•	Password setting are stored in xml file
•	Client side and server side validation
•	Password strength in different colors
•	Check if password contains x number of Uppercase characters (A-Z)
•	Check if password contains any Lowercase characters (a-z)
•	Check if password contains x number of Base 10 digits (0 through 9)
•	Check if password contains x number of allowable Nonalphanumeric characters
•	Check if password meet the Minimum and Maximum password length requirement
•	Check if password exceeded the allowable Maximum consecutive repeated character
•	Check if password contains keyboard sequence (i.e., 123456, qwerty, …)

Source location: https://github.com/bryiantan/PasswordStrengthIndicator
Demo: http://download.ysatech.com/ASP-NET-jQuery-Password-Strength-v2/

This plugin depends on: jQuery 1.4 and above, jquery.blockUI (optional)

Password setting is in PasswordPolicy.xml:
duration - password age, expired in xxx days (example, not in use)
minLength - password minimum length
maxLength - password maximum length
numsLength - minimum number of required digits  
upperLength - minimum number of required upper case 
specialLength - minimum number of required special characters 
specialChars - allowable special characters
barWidth - set the bar indicator width
barColor - the bar indicator colors
useMultipleColors - 1=Yes, 0=No
maxConsecutiveRepeatedChars - 0=allow repeat, 1..n = allow 1..n maximum number of successive repetitions of a given character (example: if 1, aa will not be valid because it repeated more than 1)
maxKeyboardSequence - maximun allowable keyboard sequence (example: if 2, 123 will not be valid, but 12 will be valid. Note the sequence defined in keyboardSequenceCharacters)
keyboardSequenceCharacters - defined the list of keyboard sequence characters (example: qwerty, or based on your requirement (example: abcde..., zyx...)

Nuget:
https://www.nuget.org/packages/PasswordStrengthIndicator/

v2.0 - Initial 
v2.1 - updated the PasswordStrengthIndicator.Core assembly to read maxKeyboardSequence and keyboardSequenceCharacters from XML file
v2.2 - Updated web.config.install.xdt to insert appsetting element if not exists