# pets
This is a sample single page application to read a json data from web and display the data in a specific order. There are multiple possible solutions available to address this requirement like a simple SPA without any backend code, only reading values using the angular and display the data.
However, assuming the business will need an API layer for dealing with data, I have taken up the approach of reading the data using API layer and handling the display logic in the UI
<br>
<br>
I have used the below as the tool and technologies<br>
&nbsp;&nbsp;&nbsp;&nbsp;1    Asp.Net Core 2.0 <br>
&nbsp;&nbsp;&nbsp;&nbsp;2    TypeScript + Angular <br>
&nbsp;&nbsp;&nbsp;&nbsp;3    MsTest <br>
&nbsp;&nbsp;&nbsp;&nbsp;4    Linq <br>

<br>
<br>
I have added the test cases for the server side code using MSTEST and the client side code with Karma

<br>
<br>
Known Issues:
The sort is done only on the names of the pet.
Used Array.Sort() to sort the names, this will have differnt sort result based of the case sensitive data. Can be rectified, need a confirmation from business on the requirement
