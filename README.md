# Data structure app

My project demonstrate using two linked generics data structures 


# Binary search tree

My generic BST can search and find element with Complexity of **Log(h)** where h is the height.
using generic key and value i can store complex data in efficient get add and delete operation.
i also added Searching with minimum and maximum value in efficient search.



<p align="center" >The order of searching item with (16,10) keys with 33% percentage accuracy.</p>
<p align="center">
  <img height="600"  src="https://user-images.githubusercontent.com/91791115/184548132-d04d3aca-7af9-4d7b-acd4-abb91c40bb98.png">
</p>


# Doubly linked list

My list implement addition like a queue and handle delete items in descending Order
the time complexity of add and remove is by **O(1)** thanks to the Prev and Next pointer in each node.



<p align="center" >The order of searching item with (16,10) keys with 33% percentage accuracy.</p>
<p align="center">
  <img width="550" src="https://user-images.githubusercontent.com/91791115/184548141-5748bb70-eed1-4056-ba85-401b4e985234.png">
</p>


# Purpose of the app

## The app should manage storage in the most efficient way

The app manage a boxes store storage and can add remove and give best offer for squared bottomed box represented by X as width and length and Y as height.
It stores the data for the boxes in doubly dimentional BST with key and value and Date descending order linked list

- Main tree has Double type key and BST type value
- Each inner BST has Double type key and Box type value 

### Getting Ienumerable BST of the ranged valid values 
https://github.com/itayG98/Box-Data-Structure-app/blob/2228db6354a857bc19bea6448301e56606ac8630/DataStructure/BST.cs#L207-L230

### Method returns best offer as an enumerable using the previos Ienumerable method 
https://github.com/itayG98/Box-Data-Structure-app/blob/2228db6354a857bc19bea6448301e56606ac8630/Model/Store.cs#L270-L290


<p align="center" >The Boxes store UI in UWP.</p>
<p align="center">
  <img width="800" src="https://user-images.githubusercontent.com/91791115/184551719-1bff48f9-0651-478d-af54-97cf367ac4e4.png"
</p>

