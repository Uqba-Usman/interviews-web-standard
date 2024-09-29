<template>
  <div class="container top-container">
    <div class="task-detail-container">
      <div class="task-header">
        <router-link to="/" class="back-icon">
          <img src="/back-icon.png" alt="Back Icon" />
        </router-link>

        <div class="task-header-content">
          <h2 class="task-title">{{ task.name }}</h2>
        </div>

        <router-link :to="`/edit-task/${task.id}`" class="edit-icon">
          <img src="/edit.png" alt="Edit Icon" />
        </router-link>
      </div>

      <div class="card task-card">
        <div class="card-body">
          <div class="task-detail-section">
            <h4 class="detail-heading">Task Description</h4>
            <p class="task-description">{{ task.description }}</p>
          </div>

          <div class="task-detail-section">
            <h4 class="detail-heading">Tags</h4>
            <div class="tag-chips">
              <span v-if="!task.tags || task.tags.length === 0">
                No tags assigned to this task.
              </span>
              <router-link
                v-for="tag in task.tags"
                :key="tag.id"
                to="/"
                class="tag-chip clickable-chip"
              >
                {{ tag.tag.name }}
              </router-link>
            </div>
          </div>

          <div class="text-end mt-4">
            <router-link class="btn btn-outline-info" to="/">
              View All Tasks
            </router-link>
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
        name: "",
        description: "",
        tags: [],
      },
      isSaving: false,
    };
  },
  created() {
    const id = this.$route.params.id;
    getTask(id)
      .then((response) => {
        const taskInfo = response.data;
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
.top-container {
  margin-top: 5rem;
}

.task-header {
  background: linear-gradient(135deg, #3a3f6a, #052e82);
  padding: 40px 20px;
  color: white;
  border-radius: 8px 8px 0 0;
  text-align: center;
  position: relative;
}

.back-icon {
  position: absolute;
  left: 20px;
  top: 50%;
  transform: translateY(-50%);
  font-size: 24px;
  color: black;
  cursor: pointer;
  transition: background-color 0.3s, border 0.3s;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #052e82;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  border: 2px solid white;
}

.back-icon:hover {
  background-color: #052e82;
  color: white;
  border-color: white;
}

.back-icon img {
  width: 20px;
  height: 20px;
  filter: invert(1);
}

.edit-icon {
  position: absolute;
  right: 20px;
  top: 50%;
  transform: translateY(-50%);
  font-size: 24px;
  color: white;
  cursor: pointer;
  transition: background-color 0.3s, border 0.3s;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: 052e82;
  width: 40px;
  height: 40px;
  border-radius: 50%;
  border: 2px solid white;
}

.edit-icon:hover {
  background-color: #052e82;
  color: white;
  border-color: white;
}

.edit-icon img {
  width: 20px;
  height: 20px;
  filter: invert(1);
}

.task-header-content {
  max-width: 800px;
  margin: 0 auto;
}

.task-subtitle {
  font-size: 18px;
  margin-top: 10px;
  color: #d1d1e0;
}

.task-card {
  margin-top: -20px;
  border-top: none;
  border-radius: 0 0 8px 8px;
}

.card-body {
  padding: 20px;
}

.task-detail-section {
  margin-bottom: 20px;
}

.detail-heading {
  font-size: 20px;
  font-weight: bold;
  color: #052e82;
}

.tag-chips {
  display: flex;
  flex-wrap: wrap;
}

.tag-chip {
  background-color: #e0e0e0;
  border-radius: 16px;
  padding: 8px 12px;
  margin: 4px;
  font-size: 14px;
  text-decoration: none;
  color: #333;
  transition: background-color 0.3s, color 0.3s;
}

.clickable-chip {
  cursor: pointer;
}

.tag-chip:hover {
  background-color: #052e82;
  color: white;
}

.btn-outline-info {
  border-color: #052e82;
  color: #052e82;
}

.btn-outline-info:hover {
  background-color: #052e82;
  color: white;
}
</style>
