---
applyTo: "Pages/**/*.cshtml, wwwroot/js/**/*.js"
---

# Frontend Instructions (Razor + JavaScript)

1. Keep Razor views clean — avoid complex logic in `.cshtml`.
2. Move business logic to backend (C#).
3. Use JavaScript only for UI interactions (DOM, events, fetch).
4. Prefer fetch API for AJAX calls instead of older libraries.
5. Keep scripts modular and reusable.
6. Use consistent naming for IDs and classes.
7. Avoid inline JavaScript inside HTML where possible.