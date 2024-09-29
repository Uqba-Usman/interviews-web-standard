<template>
    <div class="container top-container">
      <div class="edit-task-container">
        <div class="card edit-task-card">
            <div class="edit-task-header">
        <button @click="goBack" class="back-icon">
          <img src="/back-icon.png" alt="Back Icon" />
        </button>
        <h2 class="text-center">Update Task</h2>
      </div>
          <div class="card-body">
            <form @submit.prevent="handleSave">
              <div class="form-group">
                <label for="name" class="form-label">Name</label>
                <input
                  v-model="task.name"
                  type="text"
                  class="form-control"
                  id="name"
                  name="name"
                  required
                />
              </div>
              <div class="form-group">
                <label for="description" class="form-label">Description</label>
                <textarea
                  v-model="task.description"
                  class="form-control"
                  id="description"
                  rows="3"
                  name="description"
                  required
                ></textarea>
              </div>
              <button
                @click="handleSave()"
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
  import Swal from 'sweetalert2';
  import { getTask, updateTask } from '~/services/taskService';
  
  export default {
    data() {
      return {
        task: {
          name: '',
          description: '',
        },
        isSaving: false,
      };
    },
    created() {
      const id = this.$route.params.id;
      getTask(id)
        .then(response => {
          const taskInfo = response.data;
          this.task.name = taskInfo.name;
          this.task.description = taskInfo.description;
        })
        .catch(error => {
          Swal.fire({
            icon: 'error',
            title: 'An Error Occurred!',
            showConfirmButton: false,
            timer: 1500,
          });
        });
    },
    methods: {
      handleSave() {
        this.isSaving = true;
        const id = this.$route.params.id;
  
        updateTask(id, this.task)
          .then(response => {
            Swal.fire({
              icon: 'success',
              title: 'Task updated successfully!',
              showConfirmButton: false,
              timer: 1500,
            });
            this.isSaving = false;
            this.task.name = '';
            this.task.description = '';
            this.$router.go(-1);

          })
          .catch(error => {
            this.isSaving = false;
            Swal.fire({
              icon: 'error',
              title: 'An Error Occurred!',
              showConfirmButton: false,
              timer: 1500,
            });
          });
      },
    },
  };
  </script>
  
  <style scoped>
  .top-container {
    margin-top: 5rem;
  }
  
  
.edit-task-header {
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

  
  .card-header {
    background: linear-gradient(135deg, #3a3f6a, #052e82);
    color: white;
    padding: 20px;
    border-radius: 8px 8px 0 0;
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
  </style>
  