<template>
    <div class="container top-container">
      <div class="tag-detail-container">

        <!-- Tag header with back button and edit button -->
        <div class="tag-header">
          <button @click="$router.go(-1)" class="back-icon">
            <img src="/back-icon.png" alt="Back Icon" />
          </button>

          <div class="tag-header-content">
            <h2 class="tag-title">{{ tag.name }}</h2>
          </div>
  
          <router-link :to="`/tags/edit/${tag.id}`" class="edit-icon">
            <img src="/edit.png" alt="Edit Icon" />
          </router-link>
        </div>
  
        <!-- Tag information card -->
        <div class="card tag-card">
          <div class="card-body">
            
            <div class="tag-detail-section">
              <h4 class="detail-heading">Assigned Tasks</h4>
              <div v-if="tag.tasks?.length" class="assigned-tasks">
                <div v-for="task in tag.tasks" :key="task.id" class="task-card">
                  <h5 class="task-name">{{ task.name }}</h5>
                  <div class="task-actions">
                    <!-- Link to view each task -->
                    <router-link
                      :to="`/tasks/show/${task.id}`"
                      class="view-task-btn"
                      >View</router-link
                    >
                  </div>
                </div>
              </div>
  
              <!-- Show message if no tasks are assigned -->
              <div v-else>
                <p>No tasks assigned to this tag.</p>
              </div>
  
            </div>
          </div>
        </div>
  
      </div>
    </div>
  </template>
  
  <script>
  import Swal from "sweetalert2";
  import { getTag } from "~/services/tagService";
  
  export default {
    data() {
      return {
        tag: {
          id: "", 
          name: "", 
        },
        isSaving: false, 
      };
    },
    created() {
      const id = this.$route.params.id;
  
      // Fetch tag details from the service
      getTag(id)
        .then((response) => {
          const tagInfo = response.data;
          // Assign tag data
          this.tag.id = tagInfo.id;
          this.tag.name = tagInfo.name;
          this.tag.tasks = tagInfo.tasks.$values || []; 
        })
        .catch((error) => {
          Swal.fire({
            icon: "error",
            title: "An Error Occurred!",
            showConfirmButton: false,
            timer: 1500,
          });
        });
    },
  };
  </script>
  
  <style scoped>
  /* Styling for the top container */
  .top-container {
    margin-top: 5rem;
  }
  
  /* Styling for the tag header */
  .tag-header {
    padding: 40px 20px;
    color: blue;
    border-radius: 8px 8px 0 0;
    text-align: center;
    position: relative;
  }
  
  /* Back button styles */
  .back-icon {
    position: absolute;
    left: 20px;
    top: 50%;
    transform: translateY(-50%);
    font-size: 24px;
    cursor: pointer;
    transition: background-color 0.3s, border 0.3s;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #052e82;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  }
  
  /* Hover effect for the back button */
  .back-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
  /* Back icon image styling */
  .back-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  
  /* Edit button styles */
  .edit-icon {
    position: absolute;
    right: 20px;
    top: 50%;
    transform: translateY(-50%);
    font-size: 24px;
    cursor: pointer;
    transition: background-color 0.3s, border 0.3s;
    display: flex;
    justify-content: center;
    align-items: center;
    background-color: #052e82;
    width: 40px;
    height: 40px;
    border-radius: 50%;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  }
  
  /* Hover effect for the edit button */
  .edit-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
  /* Edit icon image styling */
  .edit-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  
  /* Styling for the tag header content */
  .tag-header-content {
    max-width: 800px;
    margin: 0 auto;
  }
  
  /* Styling for the tag title */
  .tag-title {
    font-size: 1.8rem;
    color: #052e82;
  }
  
  /* Styling for the card component */
  .tag-card {
    margin-top: -20px;
    border-radius: 12px;
    border: none;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  }
  
  /* Card body padding */
  .card-body {
    padding: 20px;
  }
  
  /* Styling for the assigned tasks section */
  .tag-detail-section {
    margin-bottom: 20px;
  }
  
  /* Task detail heading */
  .detail-heading {
    font-size: 20px;
    font-weight: bold;
    color: #052e82;
  }
  
  /* Styling for the task cards */
  .assigned-tasks {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
  }
  
  /* Individual task card styling */
  .task-card {
    background-color: #f9f9f9;
    padding: 20px;
    border-radius: 8px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
    width: calc(33.33% - 20px);
    min-width: 200px;
    transition: box-shadow 0.3s ease;
  }
  
  /* Hover effect for task card */
  .task-card:hover {
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
  }
  
  /* Task name styling */
  .task-name {
    font-size: 16px;
    font-weight: bold;
    color: #052e82;
    margin-bottom: 10px;
  }
  
  /* Task action buttons */
  .task-actions {
    display: flex;
    justify-content: flex-end;
  }
  
  /* View task button styling */
  .view-task-btn {
    background-color: #052e82;
    color: white;
    padding: 8px 12px;
    border-radius: 4px;
    text-decoration: none;
    transition: background-color 0.3s;
  }
  
  /* Hover effect for the view task button */
  .view-task-btn:hover {
    background-color: #041f66;
  }
  </style>
  