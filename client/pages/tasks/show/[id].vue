<template>
    <div class="container top-container">
      <div class="task-detail-container">
        <!-- Task header with back button and edit button -->
        <div class="task-header">
          <button @click="$router.go(-1)" class="back-icon">
            <img src="/back-icon.png" alt="Back Icon" />
          </button>
  
          <div class="task-header-content">
            <h2 class="task-title">{{ task.name }}</h2>
          </div>
  
          <router-link :to="`/tasks/edit/${task.id}`" class="edit-icon">
            <img src="/edit.png" alt="Edit Icon" />
          </router-link>
        </div>
  
        <div class="card task-card">
          <div class="card-body">
            <!-- Section for task description -->
            <div class="task-detail-section">
              <h4 class="detail-heading">Task Description</h4>
              <p class="task-description">{{ task.description }}</p>
            </div>
  
            <!-- Section for displaying tags assigned to the task -->
            <div class="task-detail-section">
              <h4 class="detail-heading">Assigned Tags</h4>
              <div class="tag-chips">
                <!-- Display message if no tags are assigned -->
                <span v-if="!task?.tags || task?.tags?.length === 0">
                  No tags assigned to this task.
                </span>
  
                <!-- Loop through task tags and create clickable tag links -->
                <router-link
                  v-for="tag in task.tags"
                  :key="tag?.tag?.id"
                  :to="`/tags/show/${tag?.tag?.id}`"
                  class="tag-chip clickable-chip"
                >
                  {{ tag?.tag?.name }}
                </router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import Swal from "sweetalert2";
  import { getTask } from "~/services/taskService";
  
  export default {
    data() {
      return {
        task: {
          id: "", 
          name: "", 
          description: "", 
          tags: [], 
        },
        isSaving: false, 
      };
    },
    created() {
      // Fetch task data when component is created
      const id = this.$route.params.id;
      getTask(id)
        .then((response) => {
          const taskInfo = response.data;
          console.log("TASK:", taskInfo);
          // Populate task data
          this.task.id = taskInfo.id;
          this.task.name = taskInfo.name;
          this.task.description = taskInfo.description;
          this.task.tags = taskInfo.taskTags?.$values || [];
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
  /* Container styling */
  .top-container {
    margin-top: 5rem;
  }
  
  /* Task header styles */
  .task-header {
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
  
  .back-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
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
  
  .edit-icon:hover {
    background-color: #052e82;
    color: white;
  }
  
  .edit-icon img {
    width: 20px;
    height: 20px;
    filter: invert(1);
  }
  
  /* Center the task title */
  .task-header-content {
    max-width: 800px;
    margin: 0 auto;
  }
  
  /* Task title styles */
  .task-title {
    font-size: 1.8rem;
    color: #052e82;
  }
  
  /* Task card styles */
  .task-card {
    margin-top: -20px;
    border-radius: 12px;
    border: none;
    box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  }
  
  /* Card body padding */
  .card-body {
    padding: 20px;
  }
  
  /* Task detail sections */
  .task-detail-section {
    margin-bottom: 20px;
  }
  
  /* Detail section heading styles */
  .detail-heading {
    font-size: 20px;
    font-weight: bold;
    color: #052e82;
  }
  
  /* Task description text styles */
  .task-description {
    color: #333;
    font-size: 1.1rem;
    line-height: 1.6;
  }
  
  /* Tag chips container */
  .tag-chips {
    display: flex;
    flex-wrap: wrap;
  }
  
  /* Styles for individual tags */
  .tag-chip {
    background-color: #052e82;
    color: white;
    border-radius: 16px;
    padding: 8px 12px;
    margin: 4px;
    font-size: 14px;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
  }
  
  .tag-chip:hover {
    background-color: #2f61c5;
  }
  
  /* Make tags clickable */
  .clickable-chip {
    cursor: pointer;
  }
  </style>
  