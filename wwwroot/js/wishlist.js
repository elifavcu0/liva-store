async function toggleWishlist(productId, buttonElement) {
  try {
    const response = await fetch(`/Wishlist/Toggle?productId=${productId}`, {
      method: "POST",
      headers: {
        // ASP.NET Core'a bunun bir arka plan isteği(AJAX) olduğunu belirtmek için özel bir başlık ekleyelim
        // Sunucu, bu başlığı görerek kullanıcıya tam bir HTML sayfası yerine sadece JSON veya kısmi HTML dönmesi gerektiğini anlayabilir.
        "X-Requested-With": "XMLHttpRequest",
      },
    });

    if (response.status === 401) {
      const currentUrl = encodeURIComponent(
        window.location.pathname + window.location.search,
      );
      window.location.href = `/Account/SignIn?ReturnUrl=${currentUrl}`;
      return;
    }
    if (!response.ok) {
      console.error(
        "Failed to toggle wishlist item. Status: ",
        response.status,
      );
      alert(
        "An error occurred while updating your wish list. Please try again.",
      );
      return;
    }
    const data = await response.json();

    if (data.success) {
      const icon =
        buttonElement.tagName === "I"
          ? buttonElement
          : buttonElement.querySelector("i");
      if (!icon) return;

      if (data.isAdded) {
        icon.classList.remove("fa-regular");
        icon.classList.add("fa-solid", "text-liva");
        updateWishlistBadge(true);
      } else {
        icon.classList.remove("fa-solid", "text-liva");
        icon.classList.add("fa-regular");
        updateWishlistBadge(false);
      }
    }
  } catch (error) {
    console.error("Error occured: ", error);
  }
}

async function removeFromWishlistPage(productId) {
  try {
    const response = await fetch(`/Wishlist/Remove?productId=${productId}`, {
      method: "POST",
      headers: {
        "X-Requested-With": "XMLHttpRequest",
      },
    });

    if (response.ok) {
      const data = await response.json();

      if (data.success) {
        const productCard = document.getElementById(
          "wishlist-card-" + productId,
        );

        if (productCard) {
          productCard.style.transition = "opacity 0.3s ease";
          productCard.style.opacity = "0";

          setTimeout(() => {
            productCard.remove();
            updateWishlistBadge(false);
            const remainingCards = document.querySelectorAll(
              '[id^="wishlist-card-"]',
            );
            if (remainingCards.length == 0) {
              const container = document.getElementById("wishlist-container");
              if (container) {
                container.innerHTML = `
                    <div class="col-12 text-center py-5 mt-4 fade-in">
                        <i class="fa-regular fa-heart fa-4x mb-3" style="color: #dee2e6;"></i>
                        <h4 class="text-muted">Your wish list is currently empty.</h4>
                        <p class="text-muted mb-4">You can add your favorite products to your favorites list to easily find them later.</p>
                        <a href="/" class="btn text-white" style="background-color: #881337;">Start Shopping</a>
                    </div>
                `;
              }
            }
          }, 300);
        }
      }
    }
  } catch (error) {
    console.log("Error occured : ", error);
  }
}

async function updateWishlistBadge(isAddition) {
  const badge = document.getElementById("wishlist-badge");
  if (!badge) return;

  let currentCount = parseInt(badge.innerText) || 0;

  if (isAddition) {
    currentCount++;
  } else {
    currentCount--;
    if (currentCount < 0) currentCount = 0;
  }
  badge.innerText = currentCount;

  if (currentCount > 0) badge.style.display = "inline-block";
  else badge.style.display = "none";
}
