(() => {
  const orderForm = document.querySelector("[data-order-form]");

  if (!orderForm) {
    return;
  }

  const totalElement = orderForm.querySelector("[data-order-total]");
  const countElement = orderForm.querySelector("[data-order-count]");
  const currency = orderForm.dataset.currency || "USD";
  const formatter = new Intl.NumberFormat(document.documentElement.lang || "en-US", {
    style: "currency",
    currency
  });

  const clamp = (value, min, max) => Math.min(Math.max(value, min), max);

  const updateSummary = () => {
    let count = 0;
    let total = 0;

    orderForm.querySelectorAll("[data-order-item]").forEach((item) => {
      const input = item.querySelector(".quantity-input");
      const lineTotal = item.querySelector("[data-line-total]");
      const price = Number.parseFloat(item.dataset.price || "0");
      const quantity = Number.parseInt(input.value || "0", 10);
      const subtotal = price * quantity;

      count += quantity;
      total += subtotal;

      if (lineTotal) {
        lineTotal.textContent = formatter.format(subtotal);
      }
    });

    if (countElement) {
      countElement.textContent = count.toString();
    }

    if (totalElement) {
      totalElement.textContent = formatter.format(total);
    }
  };

  orderForm.querySelectorAll("[data-order-item]").forEach((item) => {
    const input = item.querySelector(".quantity-input");

    item.querySelectorAll("[data-quantity-step]").forEach((button) => {
      button.addEventListener("click", () => {
        const step = Number.parseInt(button.dataset.quantityStep || "0", 10);
        const min = Number.parseInt(input.min || "0", 10);
        const max = Number.parseInt(input.max || "9999", 10);
        const current = Number.parseInt(input.value || "0", 10);

        input.value = clamp(current + step, min, max).toString();
        input.dispatchEvent(new Event("input", { bubbles: true }));
      });
    });

    input.addEventListener("input", () => {
      const min = Number.parseInt(input.min || "0", 10);
      const max = Number.parseInt(input.max || "9999", 10);
      const current = Number.parseInt(input.value || "0", 10);

      input.value = clamp(Number.isNaN(current) ? 0 : current, min, max).toString();
      updateSummary();
    });
  });

  updateSummary();
})();
