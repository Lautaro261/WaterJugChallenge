# API Water Jug Challenge

## Algorithm

This project presents an efficient solution to the classic water jugs problem, where the goal is to measure a specific amount of water using two jugs with different capacities (Jug X and Jug Y). The algorithm uses a Breadth-First Search strategy, starting from the initial state (both jugs empty) and systematically exploring all possible reachable states through operations like "Fill," "Empty," and "Transfer" water between the jugs. The use of Breadth-First Search ensures that the first solution found is the most optimal in terms of the least number of required steps.

Additionally, the algorithm incorporates the mathematical concept of Greatest Common Divisor (GCD) to determine if the desired amount of water is achievable given the capacities of the water jugs. This preliminary check optimizes performance by avoiding the exploration of states that cannot lead to the desired solution. The combination of BFS and GCD provides an efficient and elegant solution to the water jugs challenge, effectively addressing both the exploration of state space and mathematical validation.

> [!NOTE]
> Devs: Within the code, you will find specific comments about the algorithm.

## Technology

* .NET 8
* ASP .NET Core Web API

## Setup

* Clone repository
* Open Visual Studio 2022
* Start the project with the "https" option.


## Requests 

**Method** : **POST** 

**Route** : https://localhost:7263/api/WaterJug

**Body** : 

```json
{
  "BucketCapacityX": 10,
  "BucketCapacityY": 2,
  "TargetAmountZ": 6
}
````

## Test cases

> [!NOTE]
> While the challenge did not require an initial state (JugX=0, JugY=0), I think it's convenient to add it for front-end development implementations; however, it can be removed if needed.
> A step counter has been added.


### Case 1


**Body**: 

```json
{
  "BucketCapacityX": 2,
  "BucketCapacityY": 6,
  "TargetAmountZ": 5
}
````
<p align="center">
  <img src="https://res.cloudinary.com/dmusnfifn/image/upload/v1704761027/portafolio/logos/iat9nhcrxwlxyhd63iyx.png" width="550" title="hover text">
</p>

### Case 2


**Body**: 

```json
{
  "BucketCapacityX": 2,
  "BucketCapacityY": 10,
  "TargetAmountZ": 8
}
````
<p align="center">
  <img src="https://res.cloudinary.com/dmusnfifn/image/upload/v1704761027/portafolio/logos/dcqkjoehahfe186ujmpi.png" width="550" title="hover text">
</p>

### Case 3

**Body**: 

```json
{
  "BucketCapacityX": 10,
  "BucketCapacityY": 2,
  "TargetAmountZ": 6
}
````
<p align="center">
  <img src="https://res.cloudinary.com/dmusnfifn/image/upload/v1704761027/portafolio/logos/ebmizg0vgdazum3pe8kd.png" width="550" title="hover text">
</p>




### Thank you very much for your time; I loved going through the whole process and the challenge. 
