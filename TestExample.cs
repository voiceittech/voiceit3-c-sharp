using System;
using System.Text.RegularExpressions;
using voiceit3API;

/// Test script for voiceit3 C# SDK
class TestExample {
    static string Extract(string json, string key) {
        var m = Regex.Match(json ?? "", "\"" + key + "\"\\s*:\\s*\"([^\"]+)\"");
        return m.Success ? m.Groups[1].Value : null;
    }

    static void Main(string[] args) {
        string apiKey = Environment.GetEnvironmentVariable("VOICEIT_API_KEY") ?? "";
        string apiToken = Environment.GetEnvironmentVariable("VOICEIT_API_TOKEN") ?? "";

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiToken)) {
            Console.WriteLine("Set VOICEIT_API_KEY and VOICEIT_API_TOKEN environment variables");
            Environment.Exit(1);
        }

        voiceit3 vi = new voiceit3(apiKey, apiToken);
        string userId = null, groupId = null;

        try {
            string u = vi.CreateUser();   Console.WriteLine("CreateUser: " + u);   userId = Extract(u, "userId");
            Console.WriteLine("GetAllUsers: " + vi.GetAllUsers());
            string g = vi.CreateGroup("Test Group"); Console.WriteLine("CreateGroup: " + g); groupId = Extract(g, "groupId");
            Console.WriteLine("GetAllGroups: " + vi.GetAllGroups());
            Console.WriteLine("GetPhrases: " + vi.GetPhrases("en-US"));
            Console.WriteLine("\nAll API calls completed successfully!");
        } finally {
            // Always clean up what this test created so the cloud account does not
            // accumulate test users/groups (runs even if an API call above threw).
            if (groupId != null) Console.WriteLine("DeleteGroup: " + vi.DeleteGroup(groupId));
            if (userId  != null) Console.WriteLine("DeleteUser: "  + vi.DeleteUser(userId));
        }
    }
}
