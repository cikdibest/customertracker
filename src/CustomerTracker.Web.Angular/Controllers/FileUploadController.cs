using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerTracker.Common.Helpers;

namespace CustomerTracker.Web.Angular.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    public class FileUploadController : Controller
    {
        public ActionResult Index()
        {
            var uploadFileListViewModels = PrepareLocalUploadFiles(UploadFileType.Image).Take(24).ToList();

            return View(uploadFileListViewModels);
        }

        private List<UploadFileListViewModel> PrepareLocalUploadFiles(UploadFileType uploadFileType)
        {
            if (uploadFileType == UploadFileType.None)
                return new List<UploadFileListViewModel>();

            var targetUploadFolderPath = AppDomain.CurrentDomain.BaseDirectory + "Uploads" + "\\" + uploadFileType.GetDescription();

            var files = Directory.GetFiles(targetUploadFolderPath, "*.*", SearchOption.AllDirectories).Select(q => new FileInfo(q)).OrderByDescending(t => t.CreationTime);

            var uploadFileListViewModels = files.Select(q => new UploadFileListViewModel()
            {
                FileName = q.Name,
                FilePath = uploadFileType.GetDescription() + "\\" + q.Directory. Name + "\\" + q.Name,
                UploadFileType = uploadFileType,
            }).ToList();

            return uploadFileListViewModels;
        }

       
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase fileUploadControl)
        {
            var fileUploader = FactoryFileUploader.GetFileUploader(fileUploadControl);

            fileUploader.Upload();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteFile(string fileName)
        {
            //var imagepath = _imageUploadFolder + fileName;

            //if (System.IO.File.Exists(imagepath))
            //    System.IO.File.Delete(imagepath);

            return RedirectToAction("Index");
        }

        public PartialViewResult GetUploadFiles(int uploadFileTypeId)
        {
            var uploadFileType = (UploadFileType)uploadFileTypeId;

            var uploadFileListViewModels = PrepareLocalUploadFiles(uploadFileType);

            return PartialView("_UploadedFilePreview", uploadFileListViewModels);
        }

    }

    public class UploadFileListViewModel
    {
        public string FileName { get; set; }

        public string FilePath { get; set; }

        public UploadFileType UploadFileType { get; set; }
    }

    public enum UploadFileType
    {
        [Description("images")]
        Image = 1,

        [Description("docs")]
        Doc = 2,

        [Description("videos")]
        Video = 3,

        [Description("others")]
        Other = 4,

        [Description("")]
        None = 99,
    }
    public class OtherFileUploader : BaseFileUploader
    {
        private string _otherUploadFolder;
        private HttpPostedFileBase _fileUploadControl;

        public OtherFileUploader(HttpPostedFileBase fileUploadControl)
            : base(fileUploadControl)
        {
            this._fileUploadControl = fileUploadControl;
            this._otherUploadFolder = BaseDirectory + "\\" + UploadFileType.Other.GetDescription() + "\\";
        }

        public override void Upload()
        {
            _fileUploadControl.SaveAs(ConfigureUploadFilePath(_otherUploadFolder));
        }
    }
    public class ImageFileUploader : BaseFileUploader
    {
        private readonly string _imageUploadFolder;
        private readonly HttpPostedFileBase _fileUploadControl;

        public ImageFileUploader(HttpPostedFileBase fileUploadControl)
            : base(fileUploadControl)
        {
            this._fileUploadControl = fileUploadControl;
            this._imageUploadFolder = BaseDirectory + "\\" + UploadFileType.Image.GetDescription() + "\\";
        }

        public override void Upload()
        {
            var imageFormat = HelperImage.GetImageFormat(Extension);

            var bitMap = HelperImage.GetImage(_fileUploadControl.InputStream);

            HelperImage.SaveImage(bitMap, ConfigureUploadFilePath(_imageUploadFolder), 600, imageFormat);
        }
    }
    public class FactoryFileUploader
    {
        public static IFileUploader GetFileUploader(HttpPostedFileBase fileUploadControl)
        {
            string extension = Path.GetExtension(fileUploadControl.FileName).ToLower();

            if (new List<string>() { ".jpg", ".png", ".jpeg", "gif", ".bmp", ".ico", ".tif", ".tiff", ".wmf" }.Contains(extension))
                return new ImageFileUploader(fileUploadControl);

            if (new List<string>() { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".txt", ".xml" }.Contains(extension))
                return new DocFileUploader(fileUploadControl);

            return new OtherFileUploader(fileUploadControl);
        }
    }
    public interface IFileUploader
    {
        void Upload();
    }
    public class DocFileUploader : BaseFileUploader
    {
        private string _docUploadFolder;
        private HttpPostedFileBase _fileUploadControl;

        public DocFileUploader(HttpPostedFileBase fileUploadControl)
            : base(fileUploadControl)
        {
            this._fileUploadControl = fileUploadControl;
            this._docUploadFolder = BaseDirectory + "\\" + UploadFileType.Doc.GetDescription() + "\\";
        }

        public override void Upload()
        {
            _fileUploadControl.SaveAs(ConfigureUploadFilePath(_docUploadFolder));
        }
    }
    public abstract class BaseFileUploader : IFileUploader
    {
        private readonly HttpPostedFileBase _fileUploadControl;

        protected readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory + "Uploads";

        protected string Extension
        {
            get
            {
                return Path.GetExtension(_fileUploadControl.FileName);
            }
        }

        protected string FolderOfDay = string.Empty;

        protected string ConfigureUploadFilePath(string targetFolder)
        {
            var imageUploadFolderPath = targetFolder + FolderOfDay;

            if (!Directory.Exists(imageUploadFolderPath))
                Directory.CreateDirectory(imageUploadFolderPath);

            string fileNameWithExtension = _fileUploadControl.FileName;

            if (File.Exists(imageUploadFolderPath + "\\" + fileNameWithExtension))
            {
                string fileNameWithoutExtension = DateTime.Now.ToString("yyyyMMddhhmmss");

                fileNameWithExtension = fileNameWithoutExtension + Extension;
            }

            return imageUploadFolderPath + "\\" + fileNameWithExtension;

        }

        public BaseFileUploader(HttpPostedFileBase fileUploadControl)
        {
            this._fileUploadControl = fileUploadControl;

            this.FolderOfDay = DateTime.Now.ToString("yyyy.MM.dd");
        }

        public abstract void Upload();
    }
    public static class HelperImage
    {

        public static void SaveImage(Bitmap img, string filePath, int lnWidth, ImageFormat imageFormat)
        {
            CreateThumbnail(img, lnWidth).Save(filePath, imageFormat);
        }

        public static Bitmap GetImage(Stream imageStream)
        {
            return new Bitmap(imageStream);
        }

        public static ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case @".bmp":
                    return ImageFormat.Bmp;

                case @".gif":
                    return ImageFormat.Gif;

                case @".ico":
                    return ImageFormat.Icon;

                case @".jpg":
                case @".jpeg":
                    return ImageFormat.Jpeg;

                case @".png":
                    return ImageFormat.Png;

                case @".tif":
                case @".tiff":
                    return ImageFormat.Tiff;

                case @".wmf":
                    return ImageFormat.Wmf;

                default:
                    throw new NotImplementedException();
            }
        }

        private static Bitmap CreateThumbnail(Bitmap img, int lnWidth)
        {
            Bitmap bmpOut = null;

            ImageFormat loFormat = img.RawFormat;

            decimal lnRatio;

            int lnNewWidth = 0;

            int lnNewHeight = 0;

            if (img.Width < lnWidth)
                return img;

            lnRatio = (decimal)lnWidth / img.Width;

            lnNewWidth = lnWidth;

            decimal lnTemp = img.Height * lnRatio;

            lnNewHeight = (int)lnTemp;


            bmpOut = new Bitmap(lnNewWidth, lnNewHeight);

            Graphics g = Graphics.FromImage(bmpOut);

            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.FillRectangle(Brushes.White, 0, 0, lnNewWidth, lnNewHeight);

            g.DrawImage(img, 0, 0, lnNewWidth, lnNewHeight);

            return bmpOut;
        }

    }
}
