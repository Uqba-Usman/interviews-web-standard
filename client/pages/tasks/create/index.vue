<template>
  <div class="container top-container">
    <!-- Header section with back button and title -->
    <div class="create-task-container">
      <div
        class="create-task-header d-flex align-items-center justify-content-start"
      >
        <button @click="goBack" class="back-icon">
          <img src="/back-icon.png" alt="Back Icon" />
        </button>
        <h2 class="text-center flex-grow-1">Create New Task</h2>
      </div>

      <!-- Form card for creating a new task -->
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

            <!-- Tag selection using multiselect dropdown -->
            <div class="form-group">
              <label for="tags" class="form-label">Tags</label>
              <div class="tag-chips mb-3">
                <!-- Display selected tags -->
                <span v-for="tag in task.tags" :key="tag.id" class="tag-chip">{{
                  tag.name
                }}</span>
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
                    <span class="custom__remove" @click.prevent="remove(option)"
                      >❌</span
                    >
                  </span>
                </template>

                <template #noResult>
                  No tags found.
                  <a href="#" @click.prevent="createTag" class="text-primary"
                    >Create new tag</a
                  >
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
import { createTask } from "~/services/taskService";
import { getTags } from "~/services/tagService";
import Multiselect from "vue-multiselect";
import Swal from "sweetalert2";

export default {
  components: { Multiselect },
  data() {
    return {
      task: {
        name: "", 
        description: "", 
        tags: [], 
      },
      loading: true, 
      selectedTags: [], 
      tags: [], 
      isSaving: false, 
    };
  },
  created() {
    this.fetchTags(); 
  },
  methods: {
    // Fetch tags from the server
    fetchTags() {
      getTags()
        .then((response) => {
          this.tags = response.data.$values || [];
        })
        .catch((error) => {
          console.error("Error fetching tags:", error);
          Swal.fire({
            icon: "error",
            title: "An Error Occurred!",
            showConfirmButton: false,
            timer: 1500,
          });
        });
    },
    // Handle form submission to create a task
    handleSave() {
      this.isSaving = true;
      this.task.tagIds = this.selectedTags.map((tag) => tag.id);

      createTask(this.task)
        .then(() => {
          Swal.fire({
            icon: "success",
            title: "Task created successfully!",
            showConfirmButton: false,
            timer: 1500,
          });

          
          this.task = { name: "", description: "", tags: [] };
          this.selectedTags = [];
          this.isSaving = false;

          this.$router.go(-1); 
        })
        .catch((error) => {
          console.error(error);
          Swal.fire({
            icon: "error",
            title: "An Error Occurred!",
            showConfirmButton: false,
            timer: 1500,
          });
          this.isSaving = false;
        });
    },
    // Go back to the previous page
    goBack() {
      this.$router.go(-1);
    },
    // Create a new tag on the fly
    createTag() {
      Swal.fire({
        title: "Create new tag",
        input: "text",
        showCancelButton: true,
        inputValidator: (value) => {
          if (!value) {
            return "Tag name cannot be empty!";
          }
          // Add new tag to the tags array
          this.tags.push({ id: this.tags.length + 1, name: value });
        },
      });
    },
  },
};
</script>

<style scoped>
/* Container margin */
.top-container {
  margin-top: 5rem;
}

/* Header styles */
.create-task-header {
  display: flex;
  align-items: center;
  color: white;
  padding: 20px 40px;
  border-radius: 8px 8px 0 0;
  text-align: center;
  position: relative;
  margin-bottom: 25px;
}

/* Title styles */
.create-task-header h2 {
  margin: 0;
  font-size: 1.5rem;
  color: #052e82;
  flex-grow: 1;
  text-align: center;
}

/* Back button styles */
.back-icon {
  margin-right: 10px;
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
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.back-icon img {
  width: 20px;
  height: 20px;
  filter: invert(1);
}

/* Card styles for the form */
.create-task-card {
  margin-top: -20px;
  border-radius: 12px;
  border: none;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  padding: 20px;
}

/* Input field styles */
.form-control {
  border: none;
  border-radius: 8px;
  box-shadow: 0 12px 18px rgba(0, 0, 0, 0.05);
  padding: 12px 15px;
  margin-bottom: 20px;
}

/* Save button styles */
.btn-save-task {
  background-color: #052e82;
  color: white;
  border: none;
  padding: 12px 20px;
  border-radius: 8px;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.btn-save-task:hover {
  background-color: #3a3f6a;
}

/* Tag chip styling */
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
  box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
}
</style>
