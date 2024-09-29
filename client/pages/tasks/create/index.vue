<template>
  <div class="container top-container">
    <div class="create-task-container">
      <div class="create-task-header">
        <!-- <router-link to="/" class="back-icon">
          <img src="/back-icon.png" alt="Back Icon" />
        </router-link> -->
        <button @click="goBack" class="back-icon">
          <img src="/back-icon.png" alt="Back Icon" />
        </button>
        <h2 class="text-center">Create New Task</h2>
      </div>

      <div class="card create-task-card">
        <div class="card-body">
          <form @submit.prevent="handleSave">
            <div class="form-group">
              <label for="name" class="form-label">Task Name</label>
              <input
                v-model="task.name"
                type="text"
                class="form-control"
                id="name"
                name="name"
                placeholder="Enter task name"
                required
              />
            </div>

            <div class="form-group">
              <label for="description" class="form-label">Task Description</label>
              <textarea
                v-model="task.description"
                class="form-control"
                id="description"
                rows="3"
                name="description"
                placeholder="Enter task description"
                required
              ></textarea>
            </div>

            <div class="form-group">
              <label for="tags" class="form-label">Tags</label>
              <div class="tag-chips mb-3">
                <span v-if="task.tags.length === 0">No tags assigned yet.</span>
                <span v-for="tag in task.tags" :key="tag.id" class="tag-chip">{{ tag.name }}</span>
              </div>
              <multiselect
                v-model="selectedTags"
                :options="tags"
                :multiple="true"
                :close-on-select="false"
                :clear-on-select="false"
                :preserve-search="true"
                placeholder="Assign tags"
                label="name"
                track-by="id"
                class="mb-3"
              >
                <template #tag="{ option, remove }">
                  <span class="custom__tag">
                    {{ option.name }}
                    <span class="custom__remove" @click.prevent="remove(option)">❌</span>
                  </span>
                </template>

                <template #noResult>
                  No tags found.
                  <a href="#" @click.prevent="createTag" class="text-primary">Create new tag</a>
                </template>
              </multiselect>
            </div>

            <button
              :disabled="isSaving"
              type="submit"
              class="btn btn-save-task mt-4"
            >
              {{ isSaving ? "Saving..." : "Save Task" }}
            </button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { createTask } from '~/services/taskService'; // Adjust imports as necessary
import {  getTags } from '~/services/tagService'; // Adjust imports as necessary
import Multiselect from 'vue-multiselect';
import Swal from 'sweetalert2';

export default {
  components: { Multiselect },
  data() {
    return {
      task: {
        name: '',
        description: '',
        tags: [],
      },
      loading: true,
      selectedTags: [], // For storing selected tags
      tags: [],
      isSaving: false,
    };
  },
  created() {
    this.fetchTags();
  },
  methods: {
    fetchTags() {
      getTags()
        .then((response) => {
          this.tags = response.data.$values || [];
        })
        .catch((error) => {
          console.error('Error fetching tags:', error);
          Swal.fire({
            icon: 'error',
            title: 'An Error Occurred!',
            showConfirmButton: false,
            timer: 1500,
          });
        });
    },
    handleSave() {
      this.isSaving = true;
      this.task.tags = this.selectedTags; // Update task tags with selected tags
      createTask(this.task)
        .then(() => {
          Swal.fire({
            icon: 'success',
            title: 'Task created successfully!',
            showConfirmButton: false,
            timer: 1500,
          });
          // Reset form
          this.task = { name: '', description: '', tags: [] };
          this.selectedTags = [];
          this.isSaving = false;
          // Redirect or perform other actions
        })
        .catch((error) => {
          console.error(error);
          Swal.fire({
            icon: 'error',
            title: 'An Error Occurred!',
            showConfirmButton: false,
            timer: 1500,
          });
          this.isSaving = false;
        });
    },
    goBack() {
      this.$router.go(-1); // Navigate back to the previous page
    },
    createTag() {
      Swal.fire({
        title: 'Create new tag',
        input: 'text',
        showCancelButton: true,
        inputValidator: (value) => {
          if (!value) {
            return 'Tag name cannot be empty!';
          }
          this.tags.push({ id: this.tags.length + 1, name: value }); // Add new tag
        },
      });
    },
  },
};
</script>

<style scoped>
/* Add your styles here, keeping in line with your existing styles */
.top-container {
  margin-top: 5rem;
}

.create-task-header {
  background: linear-gradient(135deg, #3a3f6a, #052e82);
  color: white;
  padding: 40px;
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


.create-task-card {
  margin-top: -20px;
  border-radius: 0 0 8px 8px;
  border-top: none;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.form-label {
  font-weight: bold;
  color: #052e82;
}

.btn-save-task {
  background-color: #052e82;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.btn-save-task:hover {
  background-color: #3a3f6a;
}

.tag-chips {
  display: flex;
  flex-wrap: wrap;
}

.tag-chip {
  background-color: #305ccc;
  color: white;
  border-radius: 16px;
  padding: 8px 12px;
  margin: 4px;
  font-size: 14px;
}
</style>
