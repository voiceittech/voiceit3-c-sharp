using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace VoiceIt3API
{
    public class VoiceIt3
    {
        const string VERSION = "4.0.0";
        string notificationUrl = "";
        RestClient client;

        // URL-encode a single path segment so caller-supplied IDs cannot
        // change the endpoint or inject query parameters.
        static string Enc(string s) => System.Uri.EscapeDataString(s ?? "");

        // Helper: send the request synchronously and return the body.
        // RestSharp 112+ is async-first but kept Execute() for sync use.
        string Send(RestRequest request)
        {
            RestResponse response = client.Execute(request);
            return response.Content ?? "";
        }

        public VoiceIt3(string apiKey, string apiToken)
            : this(apiKey, apiToken, "https://api.voiceit.io", proxy: null) { }

        public VoiceIt3(string apiKey, string apiToken, string customUrl)
            : this(apiKey, apiToken, customUrl, proxy: null) { }

        // Overloaded constructor for proxy support
        public VoiceIt3(string apiKey, string apiToken, string proxyUrl, int proxyPort)
            : this(apiKey, apiToken, "https://api.voiceit.io",
                   proxy: new WebProxy(proxyUrl, proxyPort)) { }

        VoiceIt3(string apiKey, string apiToken, string baseUrl, IWebProxy proxy)
        {
            var options = new RestClientOptions(baseUrl)
            {
                Authenticator = new HttpBasicAuthenticator(apiKey, apiToken),
                MaxTimeout = 30000,
            };
            if (proxy != null) options.Proxy = proxy;
            client = new RestClient(options);
            client.AddDefaultHeader("platformId", "30");
            client.AddDefaultHeader("platformVersion", VERSION);
        }

        public void AddNotificationUrl(String url)
        {
          notificationUrl = url;
        }

        public void RemoveNotificationUrl()
        {
          notificationUrl = "";
        }

        public string GetNotificationUrl()
        {
          return notificationUrl;
        }

        public string GetVersion()
        {
          return VERSION;
        }

        public string GetPhrases(String contentLanguage)
        {
            var request = new RestRequest
            {
                Resource = "/phrases/" + Enc(contentLanguage),
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GetAllUsers()
        {
            var request = new RestRequest
            {
                Resource = "/users",
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateUnmanagedSubAccount(string firstName, string lastName, string email, string password, string contentLanguage)
        {
            var request = new RestRequest
            {
                Resource = "/subaccount/unmanaged",
                Method = Method.Post
            };

            if(firstName != "")
                request.AddParameter("firstName", firstName);
            if(lastName != "")
                request.AddParameter("lastName", lastName);
            if(email != "")
                request.AddParameter("email", email);
            if(password != "")
                request.AddParameter("password", password);
            if(contentLanguage != "")
                request.AddParameter("contentLanguage", contentLanguage);
            
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateManagedSubAccount(string firstName, string lastName, string email, string password, string contentLanguage)
        {
            var request = new RestRequest
            {
                Resource = "/subaccount/managed",
                Method = Method.Post
            };

            if(firstName != "")
                request.AddParameter("firstName", firstName);
            if(lastName != "")
                request.AddParameter("lastName", lastName);
            if(email != "")
                request.AddParameter("email", email);
            if(password != "")
                request.AddParameter("password", password);
            if(contentLanguage != "")
                request.AddParameter("contentLanguage", contentLanguage);
            
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }


        public string RegenerateSubAccountAPIToken(string subAccountAPIKey)
        {
            var request = new RestRequest
            {
                Resource = "/subaccount/" + Enc(subAccountAPIKey),
                Method = Method.Post
            };
            
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string DeleteSubAccount(string subAccountAPIKey)
        {
            var request = new RestRequest
            {
                Resource = "/subaccount/" + Enc(subAccountAPIKey),
                Method = Method.Delete
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateUser()
        {
            var request = new RestRequest
            {
                Resource = "/users",
                Method = Method.Post
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CheckUserExists(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/users/" + Enc(userId),
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string DeleteUser(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/users/" + Enc(userId),
                Method = Method.Delete
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GetGroupsForUser(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/users/" + Enc(userId) + "/groups",
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GetAllGroups()
        {
            var request = new RestRequest
            {
                Resource = "/groups",
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GetGroup(string groupId)
        {
            var request = new RestRequest
            {
                Resource = "/groups/" + Enc(groupId),
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GroupExists(string groupId)
        {
            var request = new RestRequest
            {
                Resource = "/groups/" + Enc(groupId) + "/exists",
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateGroup(string description)
        {
            var request = new RestRequest
            {
                Resource = "/groups",
                Method = Method.Post
            };
            request.AddParameter("description", description);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string AddUserToGroup(string groupId, string userId)
        {
            var request = new RestRequest
            {
                Resource = "/groups/addUser",
                Method = Method.Put
            };
            request.AddParameter("groupId", groupId);
            request.AddParameter("userId", userId);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string RemoveUserFromGroup(string groupId, string userId)
        {
            var request = new RestRequest
            {
                Resource = "/groups/removeUser",
                Method = Method.Delete
            };
            request.AddQueryParameter("groupId", groupId);
            request.AddQueryParameter("userId", userId);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string DeleteGroup(string groupId)
        {
            var request = new RestRequest
            {
                Resource = "/groups/" + Enc(groupId),
                Method = Method.Delete
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GetAllVoiceEnrollments(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/voice/" + Enc(userId),
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GetAllFaceEnrollments(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/face/" + Enc(userId),
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string GetAllVideoEnrollments(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/video/" + Enc(userId),
                Method = Method.Get
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateVoiceEnrollment(string userId, string contentLanguage, string phrase, string recordingPath)
        {
            return CreateVoiceEnrollment(userId, contentLanguage, phrase, File.ReadAllBytes(recordingPath));
        }

        public string CreateVoiceEnrollment(string userId, string contentLanguage, string phrase, byte[] recording)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/voice",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddFile("recording", recording, "recording");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateVoiceEnrollmentByUrl(string userId, string contentLanguage, string phrase, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/voice/byUrl",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddParameter("fileUrl", fileUrl);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateFaceEnrollment(string userId, string videoPath)
        {
            return CreateFaceEnrollment(userId, File.ReadAllBytes(videoPath));
        }

        public string CreateFaceEnrollment(string userId, byte[] video)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/face",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddFile("video", video, "video");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateFaceEnrollmentByUrl(string userId, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/face/byUrl",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("fileUrl", fileUrl);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateVideoEnrollment(string userId, string contentLanguage, string phrase, string videoPath)
        {
            return CreateVideoEnrollment(userId, contentLanguage, phrase, File.ReadAllBytes(videoPath));
        }

        public string CreateVideoEnrollment(string userId, string contentLanguage, string phrase, byte[] video)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/video",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddFile("video", video, "video");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateVideoEnrollmentByUrl(string userId, string contentLanguage, string phrase, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/video/byUrl",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddParameter("fileUrl", fileUrl);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string DeleteAllEnrollments(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/enrollments/" + Enc(userId) + "/all",
                Method = Method.Delete
            };
            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VoiceVerification(string userId, string contentLanguage, string phrase, string recordingPath)
        {
            return VoiceVerification(userId, contentLanguage, phrase, File.ReadAllBytes(recordingPath));
        }

        public string VoiceVerification(string userId, string contentLanguage, string phrase, byte[] recording)
        {
            var request = new RestRequest
            {
                Resource = "/verification/voice",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddFile("recording", recording, "recording");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VoiceVerificationByUrl(string userId, string contentLanguage, string phrase, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/verification/voice/byUrl",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddParameter("fileUrl", fileUrl);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string FaceVerification(string userId, string videoPath)
        {
            return FaceVerification(userId, File.ReadAllBytes(videoPath));
        }

        public string FaceVerification(string userId, byte[] video)
        {
            var request = new RestRequest
            {
                Resource = "/verification/face",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddFile("video", video, "video");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string FaceVerificationByUrl(string userId, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/verification/face/byUrl",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("fileUrl", fileUrl);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VideoVerification(string userId, string contentLanguage, string phrase, string videoPath)
        {
            return VideoVerification(userId, contentLanguage, phrase, File.ReadAllBytes(videoPath));
        }

        public string VideoVerification(string userId, string contentLanguage, string phrase, byte[] video)
        {
            var request = new RestRequest
            {
                Resource = "/verification/video",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddFile("video", video, "video");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VideoVerificationByUrl(string userId, string contentLanguage, string phrase, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/verification/video/byUrl",
                Method = Method.Post
            };
            request.AddParameter("userId", userId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("fileUrl", fileUrl);
            request.AddParameter("phrase", phrase);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VoiceIdentification(string groupId, string contentLanguage, string phrase, string recordingPath)
        {
            return VoiceIdentification(groupId, contentLanguage, phrase, File.ReadAllBytes(recordingPath));
        }

        public string VoiceIdentification(string groupId, string contentLanguage, string phrase, byte[] recording)
        {
            var request = new RestRequest
            {
                Resource = "/identification/voice",
                Method = Method.Post
            };
            request.AddParameter("groupId", groupId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddFile("recording", recording, "recording");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VoiceIdentificationByUrl(string groupId, string contentLanguage, string phrase, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/identification/voice/byUrl",
                Method = Method.Post
            };
            request.AddParameter("groupId", groupId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddParameter("fileUrl", fileUrl);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string FaceIdentification(string groupId, string videoPath) {
            return FaceIdentification(groupId, File.ReadAllBytes(videoPath));
        }

        public string FaceIdentification(string groupId, byte[] video)
        {
            var request = new RestRequest
            {
                Resource = "/identification/face",
                Method = Method.Post
            };
            request.AddParameter("groupId", groupId);
            request.AddFile("video", video, "video");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string FaceIdentificationByUrl(string groupId, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/identification/face/byUrl",
                Method = Method.Post
            };
            request.AddParameter("groupId", groupId);
            request.AddParameter("fileUrl", fileUrl);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VideoIdentification(string groupId, string contentLanguage, string phrase, string videoPath)
        {
            return VideoIdentification(groupId, contentLanguage, phrase, File.ReadAllBytes(videoPath));
        }

        public string VideoIdentification(string groupId, string contentLanguage, string phrase, byte[] video)
        {
            var request = new RestRequest
            {
                Resource = "/identification/video",
                Method = Method.Post
            };
            request.AddParameter("groupId", groupId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("phrase", phrase);
            request.AddFile("video", video, "video");

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string VideoIdentificationByUrl(string groupId, string contentLanguage, string phrase, string fileUrl)
        {
            var request = new RestRequest
            {
                Resource = "/identification/video/byUrl",
                Method = Method.Post
            };
            request.AddParameter("groupId", groupId);
            request.AddParameter("contentLanguage", contentLanguage);
            request.AddParameter("fileUrl", fileUrl);
            request.AddParameter("phrase", phrase);

            if (notificationUrl != "")
            {
              request.AddParameter("notificationURL", notificationUrl);
            }
            return Send(request);
        }

        public string CreateUserToken(string userId, int secondsToTimeout)
        {
            var request = new RestRequest
            {
                Resource = "/users/" + Enc(userId) + "/token",
                Method = Method.Post
            };
            request.AddParameter("timeOut", secondsToTimeout.ToString());
            return Send(request);
        }

        public string ExpireUserTokens(string userId)
        {
            var request = new RestRequest
            {
                Resource = "/users/" + Enc(userId) + "/expireTokens",
                Method = Method.Post
            };
            return Send(request);
        }

    }
}
