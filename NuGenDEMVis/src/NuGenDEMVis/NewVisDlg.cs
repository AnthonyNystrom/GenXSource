using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Genetibase.NuGenDEMVis.Data;
using Genetibase.NuGenDEMVis.GIS;

namespace Genetibase.NuGenDEMVis.UI
{
    /// <summary>
    /// Dialog for specifying how to create a visualization from an image
    /// </summary>
    partial class NewVisDlg : Form
    {
        readonly FileType[] fileTypes;
        readonly DataProfile[] dataProfiles;
        DataProfile[] compatibleProfiles;

        IDataSourceReader reader;

        readonly string filePath;
        FileType fType;
        DataSourceInfo dsInfo;

        public NewVisDlg(string file, FileType[] fTypes, DataProfile[] dProfiles)
        {
            InitializeComponent();

            filePath = file;

            fileTypes = fTypes;
            dataProfiles = dProfiles;

            ReadDataSource(file);
        }

        private void ReadDataSource(string file)
        {
            // look for file-type
            foreach (FileType type in fileTypes)
            {
                if (type.IsFileType(file))
                {
                    fType = type;
                    break;
                }
            }
            if (fType == null)
                throw new Exception(String.Format("Unable to match file type for {0}", fType));
            
            // load data-source
            reader = new GDALReader();
            reader.OpenFile(file, fType);
            
            // build source layers for UI
            dsInfo = reader.Info;
            DataSourceControl.DataSourcePreviewInfo info = new DataSourceControl.DataSourcePreviewInfo();
            info.Dimensions = dsInfo.Resolution;
            info.Type = fType.Name;
            info.DataType = dsInfo.BppType;
            info.Groups = new DataSourceControl.DataSourceImageGroup[2];
            DataSourceControl.DataSourcePreviewImage[] images = new DataSourceControl.DataSourcePreviewImage[dsInfo.Bands.Length];
            for (int i = 0; i < dsInfo.Bands.Length; i++)
            {
                images[i] = new DataSourceControl.DataSourcePreviewImage(dsInfo.Bands[i].Image,
                                                                         dsInfo.Bands[i].Name,
                                                                         new Size(64, 64));
            }
            DataSourceControl.DataSourcePreviewImage[] buildInImages = new DataSourceControl.DataSourcePreviewImage[2];
            buildInImages[0] = new DataSourceControl.DataSourcePreviewImage(null, "NormalMap", new Size(64, 64));
            buildInImages[1] = new DataSourceControl.DataSourcePreviewImage(/*Image.FromFile(file)*/null, "TextureMap", new Size(64, 64));

            info.Groups[1] = new DataSourceControl.DataSourceImageGroup(images);
            info.Groups[0] = new DataSourceControl.DataSourceImageGroup(buildInImages);
            dataSourceControl1.DataSource = info;
            
            // load profiles for data-type
            List<DataProfile> profiles = new List<DataProfile>();
            foreach (DataProfile profile in dataProfiles)
            {
                if (profile.MatchProfile(fType))
                    profiles.Add(profile);
            }
            compatibleProfiles = profiles.ToArray();

            dataProfileControl1.Profiles = compatibleProfiles;
        }

        private void uiClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Properties

        public string VisName
        {
            get { return Path.GetFileName(filePath); }
        }

        public FileType VisFileType
        {
            get { return fType; }
        }

        public string VisFileName
        {
            get { return filePath; }
        }

        public DataProfile VisDataProfile
        {
            get { return dataProfileControl1.SelectedProfile; }
        }

        public DataProfile.SubProfile VisSubDataProfile
        {
            get { return dataProfileControl1.SelectedSubProfile; }
        }

        public DataSourceInfo VisDataSourceInfo
        {
            get { return dsInfo; }
        }

        public IDataSourceReader VisDataReader
        {
            get { return reader; }
            set { reader = value; }
        }
        #endregion
    }
}