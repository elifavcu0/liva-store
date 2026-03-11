async function toggleWishlist(productId, buttonElement) {
  try {
    const response = await fetch(`/Wishlist/Toggle?productId=${productId}`, {
      method: "POST",
      headers:{
        // ASP.NET Core'a bunun bir arka plan isteği olduğunu belirtmek için özel bir başlık ekleyelim
        "X-Requested-With": "XMLHttpRequest"
      }
    });

    if(response.status === 401) {
      const currentUrl = encodeURIComponent(window.location.pathname+window.location.search);
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
        icon.classList.add("fa-solid");
        icon.style.color = "red";
      } else {
        icon.classList.remove("fa-solid");
        icon.classList.add("fa-regular");
        icon.style.color = ""; 
      }
    }
  } catch (error) {
    console.error("Error occured: ", error);
  }
}
