﻿using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Crm.Sdk.Messages;
using Xrm.Framework.CI.Common;

namespace Xrm.Framework.CI.PowerShell.Cmdlets
{
    /// <summary>
    /// <para type="synopsis">Expands data zip file generated by Configuration Migration Tool</para>
    /// <para type="description">Expands the data zip files and optionally created a file per entity
    /// </para>
    /// </summary>
    /// <example>
    ///   <code>C:\PS>Expand-XrmSolution -ConnectionString "" -EntityName "account"</code>
    ///   <para>Exports the "" managed solution to "" location</para>
    /// </example>
    [Cmdlet(VerbsData.Expand, "XrmCMData")]
    public class ExpandXrmCMData : CommandBase
    {
        #region Parameters

        /// <summary>
        /// <para type="description">The absolute path to the data zip file</para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public string DataZip { get; set; }

        /// <para type="description">The target folder for the extracted files</para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Folder { get; set; }

        /// <summary>
        /// <para type="description">Splits the xml data into mutiple files per entity</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool SplitDataXmlFile { get; set; }

        /// <summary>
        /// <para type="description">Determines the level to which the xml data is split in the folder structure</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateSet("Default", "None", "EntityLevel", "FileLevel")]
        [PSDefaultValue(Value="Default")]
        public string SplitDataXmlFileLevel { get; set; }

        /// <summary>
        /// <para type="description">Sorts the data xml file based on record ids</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        public bool SortDataXmlFile { get; set; }

        #endregion

        #region Process Record

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            Logger.LogInformation("Expanding data file {0} to path: {1}", DataZip, Folder);

            ConfigurationMigrationManager manager = new ConfigurationMigrationManager(Logger);

            manager.ExpandData(DataZip, Folder);

            if (SortDataXmlFile)
            {
                manager.SortDataXml(Folder);
            }

            var splitDataXmlFileLevelType = (CmExpandTypeEnum)Enum.Parse(typeof(CmExpandTypeEnum), SplitDataXmlFileLevel);
            
            if (SplitDataXmlFile || splitDataXmlFileLevelType != CmExpandTypeEnum.None)
            {
                manager.SplitData(Folder, splitDataXmlFileLevelType);
            }

            Logger.LogInformation("Extracting Data Completed");
        }

        #endregion
    }
}
