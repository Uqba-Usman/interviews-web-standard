<template>
    <!-- Navbar with Bootstrap styling -->
    <nav class="navbar navbar-expand-lg w-100" style="background-color: #052e82;">
      <div class="container-fluid">
        <!-- Brand logo linking to homepage -->
        <a class="navbar-brand text-logo" href="/">Infineon</a>
  
        <!-- Button for toggling navbar on smaller screens -->
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <!-- Icon displayed on toggle button -->
          <span class="navbar-toggler-icon"></span>
        </button>
  
        <!-- Collapsible part of the navbar containing navigation links -->
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav ms-auto pe-4">
            <!-- Navigation item for "Tasks" page -->
            <li class="nav-item">
              <router-link
                to="/"
                class="nav-link text-white button-hover"
                :class="{ active: isActive('/') }"
                >Tasks</router-link
              >
            </li>
            <!-- Navigation item for "Tags" page -->
            <li class="nav-item">
              <router-link
                to="/tags"
                class="nav-link text-white button-hover"
                :class="{ active: isActive('/tags') }"
                >Tags</router-link
              >
            </li>
          </ul>
        </div>
      </div>
    </nav>
  </template>
  
  
  
  <script>
  export default {
    methods: {
      isActive(route) {
         // Check if the current route matches the given one
        return this.$route.path === route;
      },
    },
  };
  </script>
  
  <style>
    /* Styling for the brand text/logo */
.text-logo {
  font-weight: 700;
  font-size: 1.8rem;
  color: white;
  letter-spacing: 1px;
  text-transform: lowercase;
}

/* Styling for the navbar toggle button on smaller screens */
.navbar-toggler {
  border-color: white; /* White border for the toggle button */
}

/* Custom icon for the navbar toggle button */
.navbar-toggler-icon {
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 30 30'%3E%3Cpath stroke='white' stroke-width='2' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
}

/* Styling for the buttons in the navbar */
.button-hover {
  padding: 10px 10px;
  border-radius: 5px;
  transition: background-color 0.3s ease, color 0.3s ease; /* Smooth color transition on hover */
  color: white; /* Initial text color */
}

/* On hover, change the background to white and the text color to blue */
.button-hover:hover {
  background-color: white;
  color: #052e82 !important;
}

/* Styling for the active link (highlighted) */
.active {
  background-color: white; /* White background for active link */
  color: #052e82 !important; /* Text color for active link */
  padding: 10px 20px !important; /* Additional padding for active link */
  border-radius: 5px; /* Rounded corners */
}

  </style>