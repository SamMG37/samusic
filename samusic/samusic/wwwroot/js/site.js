document.addEventListener("DOMContentLoaded", function () {
    const sections = document.querySelectorAll("section");

    sections.forEach(function (section) {
        const text = section.textContent.trim();

        if (
            text.includes("Use another service to log in.") ||
            text.includes("Use another service to register.")
        ) {
            const rightColumn =
                section.closest(".col-md-4") ||
                section.closest(".col-md-6") ||
                section.parentElement;

            if (rightColumn) {
                rightColumn.style.display = "none";
            }

            const leftColumn =
                document.querySelector(".col-md-6") ||
                document.querySelector(".col-md-8");

            if (leftColumn) {
                leftColumn.classList.remove("col-md-6", "col-md-8");
                leftColumn.classList.add("col-md-12");
            }
        }
    });
});
