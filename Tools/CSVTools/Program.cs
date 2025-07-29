using CSVTools;

string pathHelperList = "C:\\git\\FireDepartmentAdministration\\Data\\AllHelper2025.csv";
string pathHelperNumbers = "C:\\git\\FireDepartmentAdministration\\Data\\AllMembersPhoneNumbers.csv";
string pathOutput = "C:\\git\\FireDepartmentAdministration\\Data\\MatchedMembersPhoneNumbers.csv";

MatchMemberPhoneNumber.RunWithCSV(pathHelperList, pathHelperNumbers, pathOutput);