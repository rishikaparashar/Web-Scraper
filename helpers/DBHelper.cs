/*
This class will help us in reading/writing data from/to the Database.
*/

using System.Collections.Generic;
using WebScraperModularized.data;
using Dapper;
using Z.Dapper.Plus;
using System.Data;
using System;
using Npgsql;
using WebScraperModularized.wrappers;

namespace WebScraperModularized.helpers
{
    public class DBHelper
    {        
        /*
        Method to return n URLs from DB.
        */
        public static IEnumerable<URL> getURLSFromDB(int n, bool initialLoad)
        {
            IEnumerable<URL> myUrlEnumerable = null;


            using (IDbConnection db = DBConnectionHelper.getConnection())
            {
                //get connection
                if (db != null)
                {
                    int[] myStatusArr = new[] {(int) URL.URLStatus.INITIAL, (int) URL.URLStatus.RUNNING};
                    if (!initialLoad)
                    {
                        //if not initial load, we need to get new urls in status INITIAL
                        myStatusArr = new[] {(int) URL.URLStatus.INITIAL};
                    }

                    myUrlEnumerable =
                        db.Query<URL>(
                            "Select top(@k) Id, Url, STATUS,PROPERTY, URL_TYPE, ZIP_CODE from URL where status IN @status",
                            new {status = myStatusArr, k = n});
                }
            }

            return myUrlEnumerable;
        }

        /*
        Method to insert parsed properties into DB
        */
        public static void insertParsedProperties(PropertyData propData)
        {
            if (propData == null) return;
            List<PropertyType> propertyTypeList = propData.urlList;
            if (propertyTypeList != null && propertyTypeList.Count > 0)
            {
                using(IDbConnection db = DBConnectionHelper.getConnection()){//get connection
                    //get property id from db if exists
                    propertyTypeList.ForEach(x => x.id = getPropTypeIdDb(x));
                    //get connection
                    db.BulkMerge(propertyTypeList) //insert the list of property types
                        .ThenForEach(x => x.properties
                            .ForEach(y => y.PROPERTY_TYPE = x.id)) //set property type id for properties
                        .ThenBulkMerge(x => x.properties) //insert properties
                        .ThenForEach(x => x.url.property = x.id) //set property id for urls
                        .ThenBulkMerge(x => x.url); //insert urls
                }
            }
        }

        //this method returns the property type id from DB
        private static int getPropTypeIdDb(PropertyType x){
            if(x!=null && x.PROPERTY_TYPE!=null){
                using(IDbConnection db = DBConnectionHelper.getConnection()){//get connection
                    PropertyType z = db.QueryFirstOrDefault<PropertyType>(
                        "Select * from PROPERTY_TYPE where PROPERTY_TYPE like @proptype", 
                        new {proptype = x.PROPERTY_TYPE}
                    );
                    if(z!=null){
                        return z.id;
                    }
                }
            }
            return 0;
        }

        //insert schools in db

        public static void insertParsedApartment(ApartmentData apartmentData)
        {
              insertParsedSchools(apartmentData.schoolsList);
              insertParsedReviews(apartmentData.reviewsList);
              insertParsedNTPI(apartmentData.NTPIList);
              insertParsedExpenseType(apartmentData.expensesTypeList);
              insertParsedApartmentList(apartmentData.apartmentsList);
              insertParsedPropertyAmenities(apartmentData.amenityTypesList);
        }


        public static void insertParsedApartmentList(List<Apartments> apartments)
        {
            if (apartments == null) return;
            if (apartments != null && apartments.Count > 0)
            {
                BulkMergeUtil(apartments);
            }
        }

        public static void insertParsedSchools(List<School> schoolsList){
            if(schoolsList==null) return;
            if(schoolsList!=null && schoolsList.Count>0){
                using(IDbConnection db = DBConnectionHelper.getConnection())
                {//get connection
                    schoolsList.ForEach(x=>x.id = getSchoolIdDb(x));
                    db.BulkMerge(schoolsList)
                        .ThenForEach(x => x.PropSchoolMapping
                            .ForEach(y => y.School = x.id))
                        .ThenBulkMerge(x => x.PropSchoolMapping);
                }
            }
        }


        private static int getSchoolIdDb(School x){
            if(x!=null){
                using(IDbConnection db = DBConnectionHelper.getConnection()){
                    School z = db.QueryFirstOrDefault<School>(
                        "Select * from school where " + 
                        "name like @name and " +
                        "type_text like @type_text and " +
                        "grades like @grades and " +
                        "No_of_students = @no_of_students and " +
                        "contact_number like @contact_number", 
                        new {
                            name = x.name, 
                            type_text = x.Type_text,
                            grades = x.grades,
                            no_of_students = x.No_of_students,
                            contact_number = x.Contact_number
                        }
                    );

                    if(z!=null) return z.id;
                }
            }
            return 0;
        }
        public static void insertParsedNTPI(List<NTPICategory> NtpiCategoryList){
            if(NtpiCategoryList==null) return;
            if(NtpiCategoryList!=null && NtpiCategoryList.Count>0){
                using(IDbConnection db = DBConnectionHelper.getConnection())
                {//get connection
                    NtpiCategoryList.ForEach(x => x.Id = getNtpiCategoryIdDb(x));
                    db.BulkMerge(NtpiCategoryList)
                        .ThenForEach(x => x.NtpiList.ForEach(y => y.NTPC = x.Id))
                        .ThenBulkMerge(x => x.NtpiList)
                        .ThenForEach(y => y.PropNTPIMapping.ForEach(z => z.NPTI = y.id))
                        .ThenBulkMerge(y => y.PropNTPIMapping);
                }
            }
        }

        private static int getNtpiIdDb(NTPI x){
            if(x!=null){
                using(IDbConnection db = DBConnectionHelper.getConnection()){
                    NTPI y = db.QueryFirstOrDefault<NTPI>(
                        "Select * from NearestTransitPointInterest where " +
                        "name like @name and " +
                        "drive like @drive"
                        );
                }
            }
            return 0;
        }

        private static int getNtpiCategoryIdDb(NTPICategory x){
            if(x!=null && x.Name!=null){
                using(IDbConnection db = DBConnectionHelper.getConnection()){
                    NTPICategory z = db.QueryFirstOrDefault<NTPICategory>(
                        "select * from NearestTransitPoint_Category where name like @name", 
                        new {name = x.Name}
                    );
                    if(z!=null){
                        return z.Id;
                    }
                }
            }
            return 0;
        }

        //insert reviews in db
        public static void insertParsedReviews(List<Review> reviewsList)
        {
            if (reviewsList == null) return;
            if (reviewsList != null && reviewsList.Count > 0)
            {
                BulkMergeUtil(reviewsList);
            }
        }

        public static void insertParsedExpenseType(List<Expensetype> expensesTypeList)
        {
            if (expensesTypeList == null) return;
            if (expensesTypeList != null && expensesTypeList.Count > 0)
            {
                //get connection
                using(IDbConnection db = DBConnectionHelper.getConnection()){
                    expensesTypeList.ForEach(x => x.id = getExpenseTypeIdDb(x));
                    //get connection
                    db.BulkMerge(expensesTypeList) //insert the list of property types
                        .ThenForEach(x => x.expensesList
                            .ForEach(y => y.Expense_Type = x.id)) //set property type id for properties
                        .ThenBulkMerge(x => x.expensesList);
                }
            }
        }


        private static int getExpenseTypeIdDb(Expensetype x){
            if(x!=null){
                using(IDbConnection db = DBConnectionHelper.getConnection()){
                    Expensetype z = db.QueryFirstOrDefault<Expensetype>(
                        "select * from expense_type where name like @name", 
                        new {name = x.Name});

                    if(z!=null){
                        return z.id;
                    }
                }
            }
            return 0;
        }
      
        public static void insertParsedPropertyAmenities(List<Amenitytype> amenityTypeList)
        {
            if (amenityTypeList == null) return;
            if (amenityTypeList != null && amenityTypeList.Count > 0)
            {
                using (IDbConnection db = DBConnectionHelper.getConnection())
                {
                    //get connection
                    db.BulkMerge(amenityTypeList)
                        .ThenForEach(x => x.amenityList
                            .ForEach(y => y.amenity_type = x.id))
                        .ThenBulkMerge(x => x.amenityList)
                        .ThenForEach(y => y.PropAmenityMapping.ForEach(z => z.Amenity = y.id))
                        .ThenBulkMerge(y => y.PropAmenityMapping);
                }
            }
        }


        public static void BulkMergeUtil<T>(List<T> list)
        {
            using (IDbConnection db = DBConnectionHelper.getConnection())
            {
                db.BulkMerge(list);
            }
        }

        /*
        This method simply merges whatever data is passed to it into DB
        */
        public static void updateURLs(Queue<URL> myUrlQueue)
        {
            using (IDbConnection db = DBConnectionHelper.getConnection())
            {
                if (db != null) db.BulkMerge(myUrlQueue);
            }
        }

        /*
        This method updates the status of url passed to it to DONE.
        */
        public static void markURLDone(URL url)
        {
            using (IDbConnection db = DBConnectionHelper.getConnection())
            {
                if (db != null)
                    db.Execute("update url set status = @status where id=@id",
                        new {status = (int) URL.URLStatus.DONE, id = url.id});
            }
        }
    }
}