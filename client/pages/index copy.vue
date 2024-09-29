<template>
        <div class="container">
            <!-- <h2 class="text-center mt-5 mb-3">Task Manager</h2> -->
            <div class="row align-items-center mt-5 mb-3">
        <div class="col">
            <h2 class="task-manager-title">Tasks</h2>
        </div>
        <div class="col text-end">
            <!-- Button to Add New Task -->
            <NuxtLink to="/create" class="btn btn-primary add-task-btn">
                +
            </NuxtLink>
        </div>
        </div>
        <div class="card">
            <div class="card-body">
                <!-- Task Table -->
                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Tags</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody v-if="!loading && tasks.length">
                        <tr v-for="task in tasks" :key="task.id">
                            <td>{{ task.name }}</td>
                            <td>
                                <button @click="openTagsModal(task)" class="show-tags-btn">Show Tags</button>
                            </td>
                            <td>
                                <NuxtLink :to="`/show/${task.id}`" class="btn btn-icon">
                                    <img src="/open.png" alt="Show" class="icon"/>
                                </NuxtLink>
                                <NuxtLink :to="`/edit/${task.id}`" class="btn btn-icon mx-1">
                                    <img src="/edit.png" alt="Edit" class="icon"/>
                                </NuxtLink>
                                <button @click="handleDelete(task.id)" class="btn btn-icon mx-1">
                                    <img src="/bin.png" alt="Delete" class="icon"/>
                                </button>
                            </td>
                        </tr>
                    </tbody>
                   
                    
                </table>

                <div v-if="loading" class="text-center my-5">
                        <div class="spinner-border" role="status"></div>
                    </div>
                <!-- Empty State -->
                <div v-if="!loading && !tasks.length" class="text-center my-5">
                    <p>No tasks available.</p>
                </div>
            </div>
        </div>

    <div v-if="isModalOpen" class="modal-overlay">
      <div class="modal-content">
        <button class="close-modal" @click="closeTagsModal">×</button>
        <h4>Tags for Task: {{ selectedTask?.name }}</h4>
        <ul>
            <li v-if="selectedTask?.tags.length === 0">No tags found.</li>
          <li v-for="tag in selectedTask?.tags" :key="tag.id">{{ tag.tag.name }}</li>
        </ul>
        <!-- <button class="btn btn-primary" @click="assignNewTags(selectedTask.id)">Assign New Tags</button> -->

        <!-- Multiselect for assigning new tags -->
        <div class="form-group mt-3">
          <label for="tags">Select Tags:</label>
          <multiselect
            v-model="selectedTags"
            :options="tags"
            :multiple="true"
            :close-on-select="false"
            :clear-on-select="false"
            :preserve-search="true"
            placeholder="Pick some tags"
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

        <button @click="handleSaveTags" :disabled="isSaving" type="button" class="btn btn-outline-primary mt-3">
          Save Tags
        </button>
      </div>
    </div>

    </div>
</template>

<script>
import { getTasks, deleteTask, updateTask } from '~/services/taskService';
import { getTags,} from '~/services/tagService';
import Swal from 'sweetalert2';

export default {
    data() {
        return {
            tasks: [],
            loading: true,
            isModalOpen: false,
            selectedTask: null,
            selectedTags: [], // For storing selected tags in the modal
            tags: [], // For all available tags
            isSaving: false,
        };
    },
    created() {
        this.fetchTaskList();
        this.fetchTags(); // Fetch tags once at component creation
    },
    methods: {
        fetchTaskList() {
            this.loading = true; 
            getTasks()
                .then((response) => {
                    console.log("RESPONSE:", response.data.$values);
                    this.tasks = response.data.$values.map(task => ({
                        ...task,
                        tags: task.taskTags?.$values || [], // Assuming tags are part of the task response
                    }));

                })
                .catch((error) => {
                    console.error(error);
                })
                .finally(() => {
                    this.loading = false; // Hide loading spinner
                });
        },
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
        handleDelete(id) {
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!',
            }).then((result) => {
                if (result.isConfirmed) {
                    deleteTask(id)
                        .then(() => {
                            Swal.fire({
                                icon: 'success',
                                title: 'Task deleted successfully!',
                                showConfirmButton: false,
                                timer: 1500,
                            });
                            this.fetchTaskList();
                        })
                        .catch((error) => {
                            Swal.fire({
                                icon: 'error',
                                title: 'An Error Occurred!',
                                showConfirmButton: false,
                                timer: 1500,
                            });
                            console.error(error);
                        });
                }
            });
        },
        openTagsModal(task) {
      this.selectedTask = task;
      this.selectedTags = task.taskTags?.$values.map(tag => ({
        id: tag.tag.id,
        name: tag.tag.name,
      })) || []; // Set previously assigned tags
      this.isModalOpen = true;
    },
    closeTagsModal() {
      this.isModalOpen = false;
      this.selectedTask = null; // Reset selected task
      this.selectedTags = []; // Reset selected tags
    },
    handleSaveTags() {
      this.isSaving = true;
      const taskId = this.selectedTask.id;
      const updatedTags = this.selectedTags.map(tag => tag.id); // Extract selected tag IDs
      
      // Prepare the updated task data
      const updatedTask = {
        tags: updatedTags,
      };

      updateTask(taskId, updatedTask)
        .then(() => {
          Swal.fire({
            icon: 'success',
            title: 'Tags updated successfully!',
            showConfirmButton: false,
            timer: 1500,
          });
          this.isSaving = false;
          this.fetchTaskList(); // Refresh task list after saving
          this.closeTagsModal(); // Close modal
        })
        .catch((error) => {
          console.error(error);
          this.isSaving = false;
          Swal.fire({
            icon: 'error',
            title: 'An Error Occurred!',
            showConfirmButton: false,
            timer: 1500,
          });
        });
    },
    createTag() {
      Swal.fire({
        title: "Create new tag",
        input: "text",
        showCancelButton: true,
        inputValidator: value => {
          if (!value) {
            return "Tag name cannot be empty!";
          }
          // Add a new tag to the list
          this.tags.push({ id: this.tags.length + 1, name: value });
        }
      });
    }
    },
};
</script>

<style scoped>
.task-manager-title {
  color: #0a3894; /* Same color as the navbar */
  font-weight: bold; /* Make it bold */
}

/* Style for Add Task button */
.add-task-btn {
  background-color: #052e82; /* Same color as the navbar */
  border-color: #052e82;
  color: white;
  /* font-weight: bold; */
}

.add-task-btn:hover {
  background-color: #041f66; /* Darker shade on hover */
  border-color: #041f66;
}

/* Style for Show Tags button */
.show-tags-btn {
  background: none;
  border: none;
  color: #052e82; /* Same color as the navbar */
  text-decoration: underline;
  cursor: pointer;
}

.show-tags-btn:hover {
  text-decoration: none; /* Remove underline on hover */
}

/* Modal Styles */

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
}

.modal-content {
  background-color: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
  width: 400px; /* Adjust modal width as needed */
  position: relative;
}

.close-modal {
  position: absolute;
  top: 10px;
  right: 10px;
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
  color: #052e82; /* Close button color */
}

.btn-icon {
  border-color: #052e82 !important; /* Custom border color */
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 5px;
  width: 40px; /* Consistent button size */
  height: 40px;
}


.btn-icon:hover {
  background-color: #6c8fd3;
  color: white;
}

.icon {
  width: 20px; 
  height: 20px;
}

.badge-info {
    background-color: #17a2b8;
    color: white;
    font-size: 0.85rem;
    padding: 5px 10px;
}

.table th, .table td {
    vertical-align: middle;
}

.spinner-border {
    width: 3rem;
    height: 3rem;
}
</style>
