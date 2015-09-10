using System.Collections.Specialized;
using System.Linq;
using System.Web;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class FileClient : IFilesClient
    {
        private readonly API _api;
        private readonly string _repoPath;

        public FileClient(API api, string repoPath)
        {
            _api = api;
            _repoPath = repoPath;
        }

        public FileData Get(string file_path, string refp)
        {
            var queryString = new NameValueCollection() { { "file_path", file_path }, { "ref", refp } };


            return _api.Get().To<FileData>(_repoPath + "/files" + ToQueryString(queryString));
        }

        public void Create(FileUpsert file)
        {
            _api.Post().With(file).Stream(_repoPath + "/files", s => { });
        }

        public void Update(FileUpsert file)
        {
            _api.Put().With(file).Stream(_repoPath + "/files", s => { });
        }

        public void Delete(FileDelete file)
        {
            _api.Delete().With(file).Stream(_repoPath + "/files", s => { });
        }


        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }
    }
}