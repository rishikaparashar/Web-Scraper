--Number of distinct zip codes scraped
select count(distinct(zip_code)) from url;

--The total number of properties that have been scraped or are scheduled to be scraped
select count(1) from property;

--The total number of properties that have been scraped and had the data for apartments
select count(distinct(property)) from apartment;

--Total number of apartments listed on apartments.com
select count(1) from apartment;

--master table with metadata for all the properties
select * from property 
where id = 34;

--get url details for the property
select * from URL
where property = 34;

--contains details for different types of apartments 
--available for the property like area, min price, 
--max price, number of beds, number of baths etc
select * from apartment 
where property = 34;

--gives details for Nearby Transit and Points of interests for a particular property
select a.Name, a.Distance, a.Drive, b.Name as Category 
from NearestTransitPointInterest a, 
NearestTransitPoint_Category b, 
NTPI_Property c
where a.id = c.id 
and a.NTPC = b.id 
and c.Property = 34;

--gives details for different amenities available for the property
select a.NAME, c.NAME as category 
from amenity a, PROPERTY_AMENITY_MAP b, AMENITY_TYPE c 
where a.id = b.AMENITY 
and a.AMENITY_TYPE = c.ID
and b.PROPERTY = 34;

--gives details for expenses for the property
select a.Name, a.Cost, b.Name as expensetype 
from Expenses a, Expense_Type b 
where b.Id = a.Expense_Type
and a.Property = 34;

--gives details for reviews for the property
select * from review where PROPERTY = 34;

--gives details for nearby schools for the property
select a.Name, a.No_of_students, a.Rating, a.Type_text, a.Grades 
from school a, Property_school b 
where a.Id = b.School 
and b.Property = 34;