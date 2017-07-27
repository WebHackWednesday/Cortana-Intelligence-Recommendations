# Cortana-Intelligence-Recommendations
In this episode, we look at the new [Cortana Intelligence Recommendations solution template](https://gallery.cortanaintelligence.com/Tutorial/Recommendations-Solution) which is a replacement for the [Cognitive Services Recommendations API](https://azure.microsoft.com/en-gb/services/cognitive-services/recommendations/) which will be retired in Feburary 2018.

## Deployment
The new solution is a template which deploys the various building blocks to your own Azure subscription. You can deploy the recommendations to your own Azure subscription from here: https://gallery.cortanaintelligence.com/Tutorial/Recommendations-Solution

## Storage and Data Files
One of the building blocks that gets deployed for you is an Azure Storage container where you upload your [Catalog](https://github.com/Microsoft/Product-Recommendations/blob/master/doc/api-reference.md#catalog-file-schema) and [Usage](https://github.com/Microsoft/Product-Recommendations/blob/master/doc/api-reference.md#usage-events-file-schema) data files. 

In the show we demonstrated this by creating a recommendations solution based around JD Sports product inventory and anonymised usage (purchase) data for the past year.

## Train the Model
When the data files are in place, you can train the recommendations model using [various trainig options and parameters](https://github.com/Microsoft/Product-Recommendations/blob/master/doc/api-reference.md#train-a-new-model). In the show, we used the recommendations UI to do this but you can also use an API call if you want to automate trainig buiods. The recommendations UI is deployed to your azure subscription as part of the solution template.

## Build your application
Once we have a trained model, we can start to build an application that uses it by passing seed item IDs and receiving recommendations base done the usage and catalog data.

In the show we build and ASP.net Core application to show how to work with the API in a very simple way. This repository contains the source code we used in the show. It consists of the main sections.

Be sure to make a note of the summary page as it contains your API keys and endpoint detailsm which you'll need later.

### Model
We created a model for serialising the JSON that the API returns

```
public class RecommendedItem
{
    public string RecommendedItemId { get; set; }
    public decimal Score { get; set; }
}
```

### Controller
We modified the default `Home` controller's `Index` action to call the API and pass the results to the default view. You shoudl update the `defaultModelUri` and `recommenderKey` with values for your own deployment.

```
public async Task <IActionResult> Index(string SeedItemId)
{
    if (string.IsNullOrEmpty(SeedItemId)) return View(new List<RecommendedItem>());

    //api details
    var defaultModelUri = "https://sportshopxmlgqkb7ew5gyws.azurewebsites.net/api/models/default/recommend";
    var recommenderKey = "M3d4d2k3amhzN21seQ==";

    //construct API url
    var parameters = new Dictionary<string, string> {
        { "recommendationCount", "10" },
        { "itemId", SeedItemId }
    };
    var apiUri = QueryHelpers.AddQueryString(defaultModelUri, parameters);

    //setup HttpClient
    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri(defaultModelUri);
    httpClient.DefaultRequestHeaders.Add("x-api-key", recommenderKey);

    //make request
    var response = await httpClient.GetAsync(apiUri);
    var responseContent = await response.Content.ReadAsStringAsync();
    var recommendedItems = JsonConvert.DeserializeObject<List<RecommendedItem>>(responseContent);

    return View(recommendedItems);
}
```

### View
Finally we modified the default `Index` view to allow the submission of a seed item id and to display the recommendation results.

```
@model List<SimpleRecommendations.Models.RecommendedItem>

<div class="row">
    <div class="col-md-12">
        <form asp-controller="Home" asp-action="Index" class="form-inline">
            <div class="form-group">
                <label for="SeedItemId">Seed Item Id:</label>
                <input type="text" class="form-control" id="SeedItemId" name="SeedItemId">
            </div>
            <button type="submit" class="btn btn-primary">Get Recommendations</button>
        </form>
    </div>
</div>

<div class="row">
    <div class="col-md-12">
        @foreach (var recommendation in Model)
        {
            <p>@recommendation.RecommendedItemId (@recommendation.Score)</p>
        }
    </div>
</div>
```

## Summary
In this episode, we looked at the new Cortana Intelligence Recommendations solution which is a powerfull, scalable way to use item-to-item recommendations base don your inventory and usage data.