/***********************************************************************************
openSIS is a free student information system for public and non-public
schools from Open Solutions for Education, Inc.Website: www.os4ed.com.

Visit the openSIS product website at https://opensis.com to learn more.
If you have question regarding this software or the license, please contact
via the website.

The software is released under the terms of the GNU Affero General Public License as
published by the Free Software Foundation, version 3 of the License.
See https://www.gnu.org/licenses/agpl-3.0.en.html.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Copyright (c) Open Solutions for Education, Inc.

All rights reserved.
***********************************************************************************/

using opensis.data.Models;
using opensis.data.ViewModels.Grades;
using System;
using System.Collections.Generic;
using System.Text;

namespace opensis.data.Interface
{
    public interface IGradeRepository
    {
        public GradeScaleAddViewModel AddGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel);
        public GradeScaleAddViewModel UpdateGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel);
        public GradeScaleAddViewModel DeleteGradeScale(GradeScaleAddViewModel gradeScaleAddViewModel);
        public GradeAddViewModel AddGrade(GradeAddViewModel gradeAddViewModel);
        public GradeAddViewModel UpdateGrade(GradeAddViewModel gradeAddViewModel);
        public GradeAddViewModel DeleteGrade(GradeAddViewModel gradeAddViewModel);
        public GradeScaleListViewModel GetAllGradeScaleList(GradeScaleListViewModel gradeScaleListViewModel);
        public GradeSortOrderModel UpdateGradeSortOrder(GradeSortOrderModel gradeSortOrderModel);
        public EffortGradeLibraryCategoryAddViewModel AddEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel);
        public EffortGradeLibraryCategoryAddViewModel UpdateEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel);
        public EffortGradeLibraryCategoryAddViewModel DeleteEffortGradeLibraryCategory(EffortGradeLibraryCategoryAddViewModel effortGradeLibraryCategoryAddViewModel);
        public EffortGradeLibraryCategoryItemAddViewModel AddEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel);
        public EffortGradeLibraryCategoryItemAddViewModel UpdateEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel);
        public EffortGradeLibraryCategoryItemAddViewModel DeleteEffortGradeLibraryCategoryItem(EffortGradeLibraryCategoryItemAddViewModel effortGradeLibraryCategoryItemAddViewModel);
        public EffortGradeLlibraryCategoryListViewModel GetAllEffortGradeLlibraryCategoryList(EffortGradeLlibraryCategoryListViewModel effortGradeLlibraryCategoryListViewModel);
        public EffortgradeLibraryCategorySortOrderModel UpdateEffortGradeLlibraryCategorySortOrder(EffortgradeLibraryCategorySortOrderModel effortgradeLibraryCategorySortOrderModel);

        public EffortGradeScaleAddViewModel AddEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel);
        public EffortGradeScaleAddViewModel UpdateEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel);
        public EffortGradeScaleAddViewModel DeleteEffortGradeScale(EffortGradeScaleAddViewModel effortGradeScaleAddViewModel);
        public EffortGradeScaleListModel GetAllEffortGradeScale(PageResult pageResult);
        public EffortGradeScaleSortOrderViewModel UpdateEffortGradeScaleSortOrder(EffortGradeScaleSortOrderViewModel effortGradeScaleSortOrderViewModel);
        public GradeUsStandardAddViewModel AddGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel);
        public GradeUsStandardAddViewModel UpdateGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel);
        public GradeUsStandardAddViewModel DeleteGradeUsStandard(GradeUsStandardAddViewModel gradeUsStandardAddViewModel);
        public GradeUsStandardListModel GetAllGradeUsStandardList(PageResult pageResult);
        public GradeUsStandardListModel GetAllSubjectStandardList(GradeUsStandardListModel gradeUsStandardListModel);
        public GradeUsStandardListModel GetAllCourseStandardList(GradeUsStandardListModel gradeUsStandardListModel);
        public CheckStandardRefNoViewModel CheckStandardRefNo(CheckStandardRefNoViewModel checkStandardRefNoViewModel);

        public HonorRollsAddViewModel AddHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel);
        public HonorRollsAddViewModel UpdateHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel);
        public HonorRollsListViewModel GetAllHonorRollList(PageResult pageResult);
        public HonorRollsAddViewModel DeleteHonorRoll(HonorRollsAddViewModel honorRollsAddViewModel);
    }
}
