<template>
  <div class="container">
    <div class="row align-items-center mt-5 mb-3">
      <div class="col">
        <h2 class="task-manager-title">Task Manager</h2>
      </div>
      <div class="col text-end">
        <NuxtLink to="/tasks/create" class="btn btn-primary add-task-btn">
          +
        </NuxtLink>
      </div>
    </div>

    <div class="card task-list-card">
      <div class="card-body">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>Task Name</th>
              <th>Tags</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody v-if="!loading && tasks.length">
            <tr
              v-for="task in tasks"
              :key="task.id"
              @click="goToTaskDetail(task.id)"
              class="clickable-row"
            >
              <td>{{ task.name }}</td>
              <td>
                <span
                  @click="openTagsModal(task)"
                  class="view-tags-text"
                  @click.stop
                >
                  View Tags
                </span>
              </td>
              <td class="actions-column">
                <!-- <NuxtLink :to="`/tasks/show/${task.id}`" class="btn-icon">
                    <img src="/open.png" alt="View" class="icon" />
                  </NuxtLink> -->
                <NuxtLink
                  :to="`/tasks/edit/${task.id}`"
                  class="btn-icon mx-1"
                  @click.stop
                >
                  <img src="/edit.png" alt="Edit" class="icon" />
                </NuxtLink>
                <button
                  @click="handleDelete(task.id)"
                  class="btn-icon"
                  @click.stop
                >
                  <img src="/bin.png" alt="Delete" class="icon" />
                </button>
              </td>
            </tr>
          </tbody>
        </table>

        <div v-if="loading" class="text-center my-5">
          <div class="spinner-border" role="status"></div>
        </div>
        <div v-if="!loading && !tasks.length" class="text-center my-5">
          <p>No tasks available.</p>
        </div>
      </div>
    </div>

    <div v-if="isModalOpen" class="modal-overlay">
      <div class="modal-content">
        <button class="close-modal" @click="closeTagsModal">×</button>
        <h4 class="modal-title">Tags for Task: {{ selectedTask?.name }}</h4>

        <div class="tag-chips mb-3">
          <span v-if="selectedTask?.tags.length === 0">No tags found.</span>
          <span
            v-for="tag in selectedTask?.tags"
            :key="tag.id"
            class="tag-chip"
          >
            {{ tag.name }}
          </span>
        </div>

        <button @click="showTagSelector = true" class="btn mt-3 add-tag-btn">
          Update Tags
        </button>

        <div v-if="showTagSelector" class="form-group mt-3">
          <label for="tags">Select Tags:</label>
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
            ><template #tag="{ option, remove }">
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
          v-if="showTagSelector"
          @click="handleSaveTags"
          :disabled="isSaving"
          class="btn mt-3 add-tag-btn"
        >
          Save Tags
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import { getTasks, deleteTask, assignTagsToTask } from "~/services/taskService";
import { getTags } from "~/services/tagService";
import Multiselect from "vue-multiselect";
import Swal from "sweetalert2";

export default {
  components: { Multiselect },
  data() {
    return {
      tasks: [],
      loading: true,
      isModalOpen: false,
      selectedTask: null,
      selectedTags: [],
      tags: [],
      showTagSelector: false,
      isSaving: false,
    };
  },
  created() {
    this.fetchTaskList();
    this.fetchTags();
  },
  methods: {
    goToTaskDetail(taskId) {
      this.$router.push(`/tasks/show/${taskId}`);
    },
    fetchTaskList() {
      this.loading = true;
      getTasks()
        .then((response) => {
          this.tasks = response.data.$values.map((task) => ({
            ...task,
            tags: task.tags?.$values || [],
          }));
        })
        .catch((error) => console.error(error))
        .finally(() => (this.loading = false));
    },
    fetchTags() {
      getTags()
        .then((response) => (this.tags = response.data.$values || []))
        .catch(() =>
          Swal.fire({ icon: "error", title: "Error fetching tags" })
        );
    },
    handleDelete(id) {
      Swal.fire({
        title: "Are you sure?",
        text: "You can't undo this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!",
      }).then((result) => {
        if (result.isConfirmed) {
          deleteTask(id)
            .then(() => {
              Swal.fire("Deleted!", "Task deleted successfully.", "success");
              this.fetchTaskList();
            })
            .catch(() =>
              Swal.fire("Error!", "Failed to delete task.", "error")
            );
        }
      });
    },
    openTagsModal(task) {
      this.selectedTask = task;
      this.selectedTags =
        task.taskTags?.$values.map((tag) => ({
          id: tag.tag.id,
          name: tag.tag.name,
        })) || [];
      this.showTagSelector = false;
      this.isModalOpen = true;
    },
    closeTagsModal() {
      this.isModalOpen = false;
      this.selectedTask = null;
      this.selectedTags = [];
    },
    handleSaveTags() {
      this.isSaving = true;
      const taskId = this.selectedTask.id;
      const updatedTags = this.selectedTags.map((tag) => tag.id);

      assignTagsToTask(taskId, { tagIds: updatedTags })
        .then(() => {
          Swal.fire("Success", "Tags updated successfully", "success");
          this.isSaving = false;
          this.closeTagsModal();
          this.fetchTaskList(); // Refresh task list
        })
        .catch(() => {
          this.isSaving = false;
          Swal.fire("Error", "Failed to update tags", "error");
        });
    },
  },
};
</script>

<style scoped>
.task-manager-title {
  font-size: 28px;
  color: #052e82;
  font-weight: bold;
}

.add-task-btn {
  background-color: #052e82;
  border: none;
  color: white;
  padding: 10px 20px;
}

.add-task-btn:hover {
  background-color: #041f66;
}

.actions-column {
  display: flex;
  align-items: center;
}

.btn-icon {
  background-color: #041f66;
  border: none;
  border-radius: 5px;
  padding: 8px;
  display: inline-flex;
  justify-content: center;
  align-items: center;
  width: 40px;
  height: 40px;
  cursor: pointer;
}

.btn-icon:hover {
  background-color: #6c8fd3;
}

.btn-icon img {
  width: 20px;
  height: 20px;
  filter: invert(1);
}

.view-tags-text {
  color: #041f66;
  text-decoration: underline;
  cursor: pointer;
  font-size: 14px;
}

.view-tags-text:hover {
  color: #305ccc;
}

.clickable-row {
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.task-list-card {
  border-radius: 12px;
}

.table th,
.table td {
  vertical-align: middle;
}

.spinner-border {
  width: 3rem;
  height: 3rem;
}

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
  width: 400px;
}

.close-modal {
  position: absolute;
  top: 10px;
  right: 10px;
  background: none;
  border: none;
  font-size: 20px;
  cursor: pointer;
  color: #052e82;
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

.show-tags-btn {
  border: none;
  background-color: transparent;
  color: #041f66;
  cursor: pointer;
}

.show-tags-btn:hover {
  color: #041f66;
}
</style>
