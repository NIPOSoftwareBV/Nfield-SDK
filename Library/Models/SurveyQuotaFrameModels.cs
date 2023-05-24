//    This file is part of Nfield.SDK.
//
//    Nfield.SDK is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Nfield.SDK is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public License
//    along with Nfield.SDK.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;

namespace Nfield.SDK.Models
{

    /// <summary>
    /// Represents survey quota frame.
    /// </summary>
    public class SurveyQuotaFrameModel
    {
        /// <summary>
        /// Returns the unique identifier of the quota frame. Ignored on the request call
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Returns the unique identifier of the quota version. Ignored on the request call
        /// </summary>
        public long? QuotaETag { get; set; }

        /// <summary>
        /// Global target of the survey quota frame
        /// </summary>
        public int? Target { get; set; }


        /// <summary>
        /// Quota Variables definitions
        /// </summary>
        public IEnumerable<SurveyQuotaVariableDefinitionModel> VariableDefinitions { get; set; }

        /// <summary>
        /// Frame Variables values
        /// </summary>
        public IEnumerable<SurveyQuotaFrameVariableModel> FrameVariables { get; set; }
    }


    /// <summary>
    /// Survey quota Variable Definition. Represents only the definition of the variable and not the assignment to the Survey
    /// </summary>
    public class SurveyQuotaVariableDefinitionModel
    {
        /// <summary>
        /// Quota Variable definition ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the quota variable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the quota variable to be used in the odin script
        /// </summary>
        public string OdinVariableName { get; set; }

        /// <summary>
        /// Makes the variable selection Optional
        /// </summary>
        public bool? IsSelectionOptional { get; set; }

        /// <summary>
        /// Allows to the variable has multible values
        /// </summary>
        public bool IsMulti { get; set; }

        /// <summary>
        /// Selectable Levels for the variable
        /// </summary>
        public IEnumerable<SurveyQuotaLevelDefinitionModel> Levels { get; set; }

    }

    /// <summary>
    /// Survey quota Level Definition. Represents only the definition of the Levvel and not the assignment to the Survey
    /// </summary>
    public class SurveyQuotaLevelDefinitionModel
    {
        /// <summary>
        /// Quota Level definition ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the quota level
        /// </summary>
        public string Name { get; set; }

    }

    /// <summary>
    /// Survey quota Frame Variable. Need to be bound to a Quota Variable Definition
    /// </summary>
    public class SurveyQuotaFrameVariableModel
    {
        /// <summary>
        /// The unique identifier of the quota variable
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Definition Id. Binds the Variable with the definition properties
        /// </summary>
        public Guid DefinitionId { get; set; }

        /// <summary>
        /// Children levels that belong to this level
        /// </summary>
        public IEnumerable<SurveyQuotaFrameLevelModel> Levels { get; set; }

        /// <summary>
        /// Hides this variable to disable the manual selection
        /// </summary>
        public bool IsHidden { get; set; }
    }


    /// <summary>
    /// Survey quota Frame Level. Need to be bound to a Quota Level DefinitionModel
    /// </summary>
    ///
    public class SurveyQuotaFrameLevelModel
    {
        /// <summary>
        /// The unique identifier of the quota level
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Definition Id. Binds the level with the definition properties
        /// </summary>
        public Guid DefinitionId { get; set; }

        /// <summary>
        /// Required quota target for this level
        /// </summary>
        public int? Target { get; set; }

        /// <summary>
        /// Max quota target for this level
        /// </summary>
        public int? MaxTarget { get; set; }

        /// <summary>
        /// Max Overshoot allowed for this level
        /// </summary>
        public int? MaxOvershoot { get; set; }

        /// <summary>
        /// Children variables that belong to this level
        /// </summary>
        public IEnumerable<SurveyQuotaFrameVariableModel> Variables { get; set; }

        /// <summary>
        /// Hides this level to disable the manual selection
        /// </summary>
        public bool IsHidden { get; set; }

    }

    /// <summary>
    /// Survey Quota Frame Level model representing a quota level targets for an specific ETag
    /// </summary>
    public class SurveyQuotaFrameEtagModel
    {
        public IEnumerable<SurveyQuotaFrameEtagLevelTargetModel> Levels { get; set; }
    }

    public class SurveyQuotaFrameEtagLevelTargetModel
    {
        /// <summary>
        /// The unique identifier of the quota level
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Required quota target for this level
        /// </summary>
        public int? Target { get; set; }

        /// <summary>
        /// Max quota target for this level
        /// </summary>
        public int? MaxTarget { get; set; }

        /// <summary>
        /// Max Overshoot allowed for this level
        /// </summary>
        public int? MaxOvershoot { get; set; }
    }

}
