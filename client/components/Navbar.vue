<template>
    <nav class="navbar navbar-expand-lg w-100" style="background-color: #052e82;">
      <div class="container-fluid">
        <a class="navbar-brand text-logo" href="/">Infineon</a>
  
        <button
          class="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span class="navbar-toggler-icon"></span>
        </button>
  
        <div class="collapse navbar-collapse" id="navbarNav">
          <ul class="navbar-nav ms-auto pe-4">
            <li class="nav-item">
              <router-link
                to="/"
                class="nav-link text-white button-hover"
                :class="{ active: isActive('/') }"
              >
                Tasks
              </router-link>
            </li>
            <li class="nav-item">
              <router-link
                to="/tags"
                class="nav-link text-white button-hover"
                :class="{ active: isActive('/tags') }"
              >
                Tags
              </router-link>
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
        return this.$route.path === route;
      },
    },
  };
  </script>
  
  <style>
  .text-logo {
    font-weight: 700;
    font-size: 1.8rem;
    color: white;
    letter-spacing: 1px;
    text-transform: lowercase;
  }
  
  .navbar-toggler {
    border-color: white;
  }
  
  .navbar-toggler-icon {
    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 30 30'%3E%3Cpath stroke='white' stroke-width='2' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
  }
  
  .button-hover {
    padding: 10px 10px;
    border-radius: 5px;
    transition: background-color 0.3s ease, color 0.3s ease;
    color: white;
  }
  
  .button-hover:hover {
    background-color: white;
    color: #052e82 !important;
  }
  
  .active {
  background-color: white; 
  color: #052e82 !important;
  padding: 10px 20px !important;
  border-radius: 5px;
}
  </style>
  