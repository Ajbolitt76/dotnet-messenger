﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Net;
using Messenger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Messenger.Data.Migrations
{
    [DbContext(typeof(MessengerContext))]
    [Migration("20230419192403_Modify")]
    partial class Modify
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("ConversationType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("HardDeletedCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasDefaultValue(0L);

                    b.Property<DateTime>("LastMessageDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationInfos.ChannelInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ChannelPictureId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId")
                        .IsUnique();

                    b.HasIndex("ConversationId", "LastUpdated")
                        .IsDescending(false, true);

                    b.ToTable("ChannelInfos");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationInfos.GroupChatInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("GroupPictureId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId")
                        .IsUnique();

                    b.HasIndex("ConversationId", "LastUpdated")
                        .IsDescending(false, true);

                    b.ToTable("GroupChatInfos");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationInfos.PersonalChatInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("InitiatorPeer")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("RecipientPeer")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId")
                        .IsUnique();

                    b.HasIndex("ConversationId", "LastUpdated")
                        .IsDescending(false, true);

                    b.ToTable("PersonalChatInfos");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Attachments")
                        .HasColumnType("jsonb");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<List<Guid>>("DeletedFrom")
                        .HasColumnType("uuid[]");

                    b.Property<DateTime?>("EditedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Metadata")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<long>("Position")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SentAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("TextContent")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId", "Position")
                        .IsUnique();

                    b.ToTable("ConversationMessages");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationUserStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<long?>("DeletedTo")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeletedByUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<long>("ReadTo")
                        .HasColumnType("bigint");

                    b.Property<long>("SoftDeletedCount")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId", "UserId")
                        .IsUnique();

                    b.ToTable("ConversationUserStatuses");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.Members.ChannelMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("Permissions")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("ConversationId", "UserId")
                        .IsUnique();

                    b.ToTable("ChannelMembers");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.Members.GroupChatMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsOwner")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("MutedTill")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Permissions")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<bool>("WasBanned")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("WasExcluded")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("ConversationId", "UserId")
                        .IsUnique();

                    b.ToTable("GroupChatMembers");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.Members.PersonalChatMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.HasIndex("UserId");

                    b.ToTable("PersonalChatMembers");
                });

            modelBuilder.Entity("Messenger.Core.Model.FileAggregate.SystemFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<IPAddress>("CreatorIp")
                        .HasColumnType("inet");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("Messenger.Core.Model.FileAggregate.UploadingFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("CreatedById")
                        .HasColumnType("uuid");

                    b.Property<IPAddress>("CreatorIp")
                        .HasColumnType("inet");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("boolean");

                    b.Property<string>("TusId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("UploadingFiles");
                });

            modelBuilder.Entity("Messenger.Core.Model.UserAggregate.MessengerUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("IdentityUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ProfilePhotoId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.HasIndex("PhoneNumber");

                    b.ToTable("MessengerUsers");
                });

            modelBuilder.Entity("Messenger.Infrastructure.User.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("RefreshTokens")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationInfos.ChannelInfo", b =>
                {
                    b.HasOne("Messenger.Core.Model.ConversationAggregate.Conversation", "Conversation")
                        .WithOne()
                        .HasForeignKey("Messenger.Core.Model.ConversationAggregate.ConversationInfos.ChannelInfo", "ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationInfos.GroupChatInfo", b =>
                {
                    b.HasOne("Messenger.Core.Model.ConversationAggregate.Conversation", "Conversation")
                        .WithOne()
                        .HasForeignKey("Messenger.Core.Model.ConversationAggregate.ConversationInfos.GroupChatInfo", "ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.ConversationInfos.PersonalChatInfo", b =>
                {
                    b.HasOne("Messenger.Core.Model.ConversationAggregate.Conversation", "Conversation")
                        .WithOne()
                        .HasForeignKey("Messenger.Core.Model.ConversationAggregate.ConversationInfos.PersonalChatInfo", "ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.Members.ChannelMember", b =>
                {
                    b.HasOne("Messenger.Core.Model.ConversationAggregate.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Messenger.Core.Model.UserAggregate.MessengerUser", "MessengerUser")
                        .WithMany("ChannelMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("MessengerUser");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.Members.GroupChatMember", b =>
                {
                    b.HasOne("Messenger.Core.Model.ConversationAggregate.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Messenger.Core.Model.UserAggregate.MessengerUser", "MessengerUser")
                        .WithMany("GroupChatMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("MessengerUser");
                });

            modelBuilder.Entity("Messenger.Core.Model.ConversationAggregate.Members.PersonalChatMember", b =>
                {
                    b.HasOne("Messenger.Core.Model.ConversationAggregate.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Messenger.Core.Model.UserAggregate.MessengerUser", "MessengerUser")
                        .WithMany("PersonalChatMembership")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");

                    b.Navigation("MessengerUser");
                });

            modelBuilder.Entity("Messenger.Core.Model.FileAggregate.SystemFile", b =>
                {
                    b.HasOne("Messenger.Core.Model.UserAggregate.MessengerUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("Messenger.Core.Model.FileAggregate.UploadingFile", b =>
                {
                    b.HasOne("Messenger.Core.Model.UserAggregate.MessengerUser", "CreatedByUser")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedByUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("Messenger.Infrastructure.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("Messenger.Infrastructure.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Messenger.Infrastructure.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("Messenger.Infrastructure.User.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Messenger.Core.Model.UserAggregate.MessengerUser", b =>
                {
                    b.Navigation("ChannelMemberships");

                    b.Navigation("GroupChatMemberships");

                    b.Navigation("PersonalChatMembership");
                });
#pragma warning restore 612, 618
        }
    }
}
