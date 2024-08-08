
function increaseCount(ProductId) {

    let countElement = document.getElementById('item-count-' + ProductId);
    var currentCount = parseInt(countElement.innerText);
    countElement.innerText = currentCount + 1;

    $.ajax({
        url: "/Customer/Cart/ChangeCount/" + ProductId,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ count: currentCount + 1 }),
        success: function (data) {
            let totalPriceElement = document.getElementById('totalPriceId');
            totalPriceElement.innerText = '$' + data.totalPrice + '.00';
        },
        error: function (xhr, status, error) {
            console.error("An error occurred: " + error);
            countElement.innerText = currentCount;
        }
    });
}
function decreaseCount(ProductId) {

    let countElement = document.getElementById('item-count-' + ProductId);
    let currentCount = parseInt(countElement.innerText);
    var countt = currentCount;
    if (currentCount != 1) {
        countt = currentCount - 1;
        countElement.innerText = countt;
    }

    $.ajax({
        url: "/Customer/Cart/ChangeCount/" + ProductId,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ count: countt }),
        success: function (data) {
            let totalPriceElement = document.getElementById('totalPriceId');
            totalPriceElement.innerText = '$' + data.totalPrice + '.00';
        },
        error: function (xhr, status, error) {
            console.error("An error occurred: " + error);
            countElement.innerText = currentCount;
        }
    });
}

function removeItem(ProductId) {

    $.ajax({
        url: "/Customer/Cart/RemoveItem/" + ProductId,
        type: "DELETE",

        success: function (data) {
            let itemElement = document.getElementById('cart-remove-' + ProductId);
            itemElement.remove();
            let totalPriceElement = document.getElementById('totalPriceId');
            totalPriceElement.innerText = data.totalPrice;
        },
        error: function () {

        }
    });
}

