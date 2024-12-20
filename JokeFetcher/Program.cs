﻿using System;
using System.Net.Http;
using System.Text.Json;
using JokeFetcher.Dtos;

// Set the base URL for the Web Service (API)
const string baseUrl = "https://official-joke-api.appspot.com/";

// Create custom options for JsonSerialiser to ignore case when deserialising
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

var client = new HttpClient { BaseAddress = new Uri(baseUrl) };

try
{
    // Send a GET request to the "random_joke" endpoint
    HttpResponseMessage response = client.GetAsync("jokes/random").Result;

    // Ensure the request was successful (throws an exception if unsuccessful)
    response.EnsureSuccessStatusCode();

    // Read the response content as a string
    string responseBody = response.Content.ReadAsStringAsync().Result;

    // Deserialise the JSON response into a joke object (ignore case on key names)
    var joke = JsonSerializer.Deserialize<Joke>(responseBody, options);

    // Check if the joke is null
    if (joke != null)
    {
        Console.WriteLine("Here's a joke for you:");
        Console.WriteLine(joke.Setup);
        Console.WriteLine(joke.Punchline);
    }
    else
    {
        Console.WriteLine("Failed to fetch a joke.");
    }
}
catch (HttpRequestException e)
{
    // Handle any HTTP request errors
    Console.WriteLine($"Request error: {e.Message}");
}

// Start with an empty list
var jokes = new List<Joke>();

try
{
    // Send a GET request to the "jokes/{type}/ten" endpoint
    HttpResponseMessage response = client.GetAsync($"jokes/ten").Result;

    // Ensure the request was successful
    response.EnsureSuccessStatusCode();

    // Read the response content as a string
    string responseBody = response.Content.ReadAsStringAsync().Result;

    // Deserialize the JSON response into a list of Joke objects
    var jokeList = JsonSerializer.Deserialize<List<Joke>>(
        responseBody, options
        );

    // Add the jokes to the list if they are not null
    if (jokeList != null)
    {
        jokes.AddRange(jokeList);
    }
}
catch (HttpRequestException e)
{
    // Handle any HTTP request errors
    Console.WriteLine($"Request error: {e.Message}");
}

// Display list of jokes
Console.WriteLine("\n10 More Jokes");
Console.WriteLine("-------------");
foreach (var joke in jokes)
{
    Console.WriteLine(joke.Setup);
    Console.WriteLine(joke.Punchline);
    Console.WriteLine();
}