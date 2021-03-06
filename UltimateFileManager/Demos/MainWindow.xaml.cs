﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using UltimateFileManagerCore;

namespace Demos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bt_selected_file_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_file_selected.Text = dialog.FileName;
            }
        }

        private void bt_renamed_Click(object sender, RoutedEventArgs e)
        {

            tb_output.Text = $"Rename a file:\ninput {tb_file_selected.Text}\noutput {tb_file_selected.Text.RenameFile(tb_new_name.Text)}";
        }

        private void bt_directory_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_directory_selected.Text = dialog.SelectedPath;
            }
        }

        private void bt_change_directory_Click(object sender, RoutedEventArgs e)
        {
            tb_output.Text = $"Change directory of file:\ninput {tb_file_selected.Text}\noutput {UltimateFile.ChangeDirectory(tb_new_directory.Text, tb_file_selected.Text)}";
        }

        private void bt_get_size_Click(object sender, RoutedEventArgs e)
        {
            FileInfo file_selected = new FileInfo(tb_file_selected.Text);
            tb_output.Text = $"Get size of file:\ninput {tb_file_selected.Text}\noutput {file_selected.Length.ToSize()}";
        }

        private void bt_size_directory_Click(object sender, RoutedEventArgs e)
        {
            tb_output_directory.Text = $"Get size of directory:\ninput {tb_directory_selected.Text}\noutput {tb_directory_selected.Text.Size().ToSize()}";
        }

        private void bt_selected_files_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = true
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = dialog.FileNames;
                lb_selected_files.ItemsSource = files.ToList();
            }
        }

        private void bt_change_directory_files_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> files = (List<string>)lb_selected_files.ItemsSource;
                lb_output_files.ItemsSource = files.ChangeDirectory(dialog.SelectedPath);
            }
        }

        private void bt_new_names_Click(object sender, RoutedEventArgs e)
        {
            string[] new_names = tb_new_names.Text.Replace("\r\n", "\n").Split('\n');
            List<string> files = (List<string>)lb_selected_files.ItemsSource;
            lb_output_files.ItemsSource = files.RenameFiles(new_names.ToList());
        }

        private void bt_change_extension_files_Click(object sender, RoutedEventArgs e)
        {
            List<string> files = (List<string>)lb_selected_files.ItemsSource;
            lb_output_files.ItemsSource = files.ChangeExtension(tb_new_extension_files.Text);
        }

        private void bt_selected_file_copy_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_file_to_copy.Text = dialog.FileName;
            }
        }

        private async void bt_copy_file_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileManager fm = new FileManager();
                fm.ProgressUpdatedEvent += Copy_ProgressUpdatedEvent;
                bool x = await fm.CopyFileAsync(
                    tb_file_to_copy.Text,
                    UltimateFile.ChangeDirectory(dialog.SelectedPath, tb_file_to_copy.Text)
                    );
            }
        }

        private void Copy_ProgressUpdatedEvent(object sender, FileProgressUpdatedArgs e)
        {
            Console.WriteLine($"{e.BytesProcessed}-{e.TotalBytes}");
            copy_file_info.Dispatcher.Invoke(() =>
            {
                copy_file_info.Content =
                $"Copy file: {System.IO.Path.GetFileName(e.OriginFile)} [{UltimateFile.ToSize(e.BytesProcessed)}/{UltimateFile.ToSize(e.TotalBytes)}]";
            });
            pb_copy_file.Dispatcher.Invoke(() =>
            {
                pb_copy_file.Value = (e.BytesProcessed * 100) / e.TotalBytes;
            });

        }

        private async void Bt_move_file_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileManager fm = new FileManager();
                fm.ProgressUpdatedEvent += Move_ProgressUpdatedEvent; ;
                bool x = await fm.MoveFileAsync(
                    tb_file_to_copy.Text,
                    UltimateFile.ChangeDirectory(dialog.SelectedPath, tb_file_to_copy.Text)
                    );
            }
        }

        private void Move_ProgressUpdatedEvent(object sender, FileProgressUpdatedArgs e)
        {
            Console.WriteLine($"{e.BytesProcessed}-{e.TotalBytes}");
            move_file_info.Dispatcher.Invoke(() =>
            {
                move_file_info.Content =
                $"Move file: {System.IO.Path.GetFileName(e.OriginFile)} [{UltimateFile.ToSize(e.BytesProcessed)}/{UltimateFile.ToSize(e.TotalBytes)}]";
            });
            pb_move_file.Dispatcher.Invoke(() =>
            {
                pb_move_file.Value = (e.BytesProcessed * 100) / e.TotalBytes;
            });
        }

        private void Btn_select_copy_files_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Multiselect = true
            };
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = dialog.FileNames;
                lb_files_selected_cp_mv.ItemsSource = files.ToList();
            }
        }

        private void Btn_select_copy_files_folder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> files = (List<string>)lb_files_selected_cp_mv.ItemsSource;
                lb_new_files_selected_cp_mv.ItemsSource = files.ChangeDirectory(dialog.SelectedPath);
            }
        }

        private async void Btn_copy_files_Click(object sender, RoutedEventArgs e)
        {
            FileManager fm = new FileManager();
            fm.ProgressUpdatedEvent += Copy_Files_ProgressUpdatedEvent;
            bool x = await fm.CopyFilesAsync(
                (List<string>)lb_files_selected_cp_mv.ItemsSource,
                (List<string>)lb_new_files_selected_cp_mv.ItemsSource);
        }

        private void Copy_Files_ProgressUpdatedEvent(object sender, FileProgressUpdatedArgs e)
        {
            Console.WriteLine($"{e.BytesProcessed}-{e.TotalBytes}");
            lb_copy_files.Dispatcher.Invoke(() =>
            {
                lb_copy_files.Content =
                $"Copy file: {System.IO.Path.GetFileName(e.OriginFile)} [{UltimateFile.ToSize(e.BytesProcessed)}/{UltimateFile.ToSize(e.TotalBytes)}]";
            });
            pb_copy_files.Dispatcher.Invoke(() =>
            {
                pb_copy_files.Value = (e.BytesProcessed * 100) / e.TotalBytes;
            });
        }

        private async void Btn_move_files_Click(object sender, RoutedEventArgs e)
        {
            FileManager fm = new FileManager();
            fm.ProgressUpdatedEvent += Move_Files_ProgressUpdatedEvent; ;
            bool x = await fm.MoveFilesAsync(
                (List<string>)lb_files_selected_cp_mv.ItemsSource,
                (List<string>)lb_new_files_selected_cp_mv.ItemsSource);
        }

        private void Move_Files_ProgressUpdatedEvent(object sender, FileProgressUpdatedArgs e)
        {
            Console.WriteLine($"{e.BytesProcessed}-{e.TotalBytes}");
            lb_move_files.Dispatcher.Invoke(() =>
            {
                lb_move_files.Content =
                $"Move file: {System.IO.Path.GetFileName(e.OriginFile)} [{UltimateFile.ToSize(e.BytesProcessed)}/{UltimateFile.ToSize(e.TotalBytes)}]";
            });
            pb_move_files.Dispatcher.Invoke(() =>
            {
                pb_move_files.Value = (e.BytesProcessed * 100) / e.TotalBytes;
            });
        }

        private void Btn_select_copy_directory_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                tb_copy_directory.Text = dialog.SelectedPath;
            }
        }

        private async void Btn_copy_directory_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileManager fm = new FileManager();
                fm.ProgressUpdatedEvent += Copy_directory_ProgressUpdatedEvent;
                bool x = await fm.CopyDirectoryAsync(
                    tb_copy_directory.Text, dialog.SelectedPath);
            }
        }

        private void Copy_directory_ProgressUpdatedEvent(object sender, FileProgressUpdatedArgs e)
        {
            Console.WriteLine($"{e.BytesProcessed}-{e.TotalBytes}");
            lb_copy_directory.Dispatcher.Invoke(() =>
            {
                lb_copy_directory.Content =
                $"Copy file: {System.IO.Path.GetFileName(e.OriginFile)} [{UltimateFile.ToSize(e.BytesProcessed)}/{UltimateFile.ToSize(e.TotalBytes)}]";
            });
            pb_copy_directory.Dispatcher.Invoke(() =>
            {
                pb_copy_directory.Value = (e.BytesProcessed * 100) / e.TotalBytes;
            });
        }

        private async void Btn_move_directory_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileManager fm = new FileManager();
                fm.ProgressUpdatedEvent += Move_directory_ProgressUpdatedEvent; ;
                bool x = await fm.MoveDirectoryAsync(
                    tb_copy_directory.Text, dialog.SelectedPath);
            }
        }

        private void Move_directory_ProgressUpdatedEvent(object sender, FileProgressUpdatedArgs e)
        {
            Console.WriteLine($"{e.BytesProcessed}-{e.TotalBytes}");
            lb_move_directory.Dispatcher.Invoke(() =>
            {
                lb_move_directory.Content =
                $"Copy file: {System.IO.Path.GetFileName(e.OriginFile)} [{UltimateFile.ToSize(e.BytesProcessed)}/{UltimateFile.ToSize(e.TotalBytes)}]";
            });
            pb_move_directory.Dispatcher.Invoke(() =>
            {
                pb_move_directory.Value = (e.BytesProcessed * 100) / e.TotalBytes;
            });
        }
    }
}
