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
    [Cmdlet(VerbsData.Merge, "XrmCMData")]
    public class MergeXrmCMData : CommandBase
    {
        #region Parameters

        /// <para type="description">Path to the folder for the files to be merged</para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Folder { get; set; }

        /// <para type="description">Path to the mapping file which describes the merges to be performed</para>
        /// </summary>
        [Parameter(Mandatory = true)]
        public string MappingFile { get; set; }

        /// <summary>
        /// <para type="description">Determines the level to which the xml data is split in the folder structure</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [ValidateSet("Default", "None", "EntityLevel", "RecordLevel")]
        [PSDefaultValue(Value="Default")]
        public string MergeDataXmlFileLevel { get; set; }

        /// <summary>
        /// <para type="description">Determines whether to care about case when comparing target filenames to the mappingfile, Important for *Nix/Windows differing file structures</para>
        /// </summary>
        [Parameter(Mandatory = false)]
        [PSDefaultValue(Value = false)]
        public bool FileMapCaseSensitive { get; set; }
        #endregion

        #region Process Record

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            Logger.LogInformation("Merging data in folder {0} using mapping: {1}", Folder, MappingFile);

            ConfigurationMigrationManager manager = new ConfigurationMigrationManager(Logger);

            CmExpandTypeEnum mergeDataXmlFileLevelType = (CmExpandTypeEnum)Enum.Parse(typeof(CmExpandTypeEnum), MergeDataXmlFileLevel);

            manager.MapData(Folder, MappingFile, mergeDataXmlFileLevelType, FileMapCaseSensitive);

            Logger.LogInformation("Data Merge Completed");
        }

        #endregion
    }
}
